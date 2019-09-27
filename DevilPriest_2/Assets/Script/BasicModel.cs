using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace dpGame
{
    public class Director : System.Object
    {
        // 单例模式，只能有一个实例，通过getInstance访问
        private static Director _instance;

        // 管理着SceneController，通过它来间接管理场景中的对象
		public SceneController currentSceneController { get; set; }

		public static Director getInstance() {
			if (_instance == null) {
				_instance = new Director ();
			}
			return _instance;
		}
    }

    public interface SceneController {
		void loadResources ();
	}

    public interface Interaction {
        void MoveBoat();
        // void Pause();
        // void Continue();
        void Restart();
        void moveCharacters(CharacterController chr);
    }

    public class Move : MonoBehaviour {
        readonly float speed = 20;
        public int state; // 0->不运动， 1->从岸边到船上的中间过程（只对人物角色有效），2->到达目的地
        Vector3 dest;
        Vector3 tmp;
        void Update()
        {
            // 从岸上到船的上空
            if (state == 1) {
                transform.position = Vector3.MoveTowards (transform.position, tmp, speed * Time.deltaTime);
                if (transform.position == tmp) {
                    state = 2;
                }
            } 
            else if (state == 2) {
                transform.position = Vector3.MoveTowards (transform.position, dest, speed * Time.deltaTime);
                if (transform.position == dest) {
                    state = 0;
                }
            }
        }
        
        public void setDest(Vector3 new_dest) {
            dest = new_dest;
            tmp = new_dest;
            state = 1;
            // 判断dest位置是否在船上（人比船高）
            if (this.transform.position.y > dest.y) {
                tmp.y = this.transform.position.y;
            }
            else if (this.transform.position.y < dest.y) {
                tmp.x = this.transform.position.x;
                tmp.y += 0.8f;
            }
            else {
                state = 2;
            }
        }

        public void init() {
            state = 0;
        }
    }

    public class CharacterController {
        GameObject character;
        ClickEvent click;
        public Move move;
        int state; // Boat = 1, bank = 0;
        int man; //Devil = 0, Priest = 1;
        int tag;

        BankController bankController;

        public CharacterController(string chr, int tag) {
            this.tag = tag;
            if (chr == "priest") {
                character = Object.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                man = 1;
            }
            else {
                character = Object.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
                man = 0;
            }
            move = character.AddComponent(typeof(Move)) as Move;
            click = character.AddComponent(typeof(ClickEvent)) as ClickEvent;
            click.setChrController(this);
        }

        public void setName(string name) {
            character.name = name;
        }

        public void setPosition(Vector3 position) {
            character.transform.position = position;
        }

        // 原来动作未分离版本
        // public void goMoving(Vector3 position) {
        //     move.setDest(position);
        // }

        public string getName() {
            return character.name;
        }

        public int getTag() {
            return tag;
        }

        public string getMan() {
            return man==0 ? "Devil" : "Priest";
        }
        public BankController getBank() {
            return bankController;
        }
        public int getState() {
            return state;
        }
        
        public GameObject getObj() {
            return this.character;
        }

        public void moveToBoat(BoatController boatController)  {
            bankController = null;
            character.transform.parent = boatController.getObj().transform;
			state = 1;
        }

        public void moveToBank(BankController bankController)  {
            this.bankController = bankController;
            character.transform.parent = null;
			state = 0;
        }

        public void init() {
            move.init();
            bankController = (Director.getInstance ().currentSceneController as Controller).leftBank;
			moveToBank (bankController);
			setPosition (bankController.getPos(tag));
			bankController.moveToBank (this);
            
        }
    }

    public class BankController {
        GameObject bank;
        Vector3 leftBank = new Vector3(-10,-3,0);
        Vector3 rightBank = new Vector3(10,-3,0);
        Vector3[] LchrPosition;
        Vector3[] RchrPosition;
        int LR;
        CharacterController[] character;

        public BankController(string LR) {
            LchrPosition = new Vector3[] {new Vector3(-6.5F, 0.5f,0), new Vector3(-7.5F,0.5f,0), new Vector3(-8.5F,0.5f,0), 
				new Vector3(-9.5F,0.5f,0), new Vector3(-10.5F,0.5f,0), new Vector3(-11.5F,0.5f,0)};
            RchrPosition = new Vector3[] {new Vector3(6.5F, 0.5f,0), new Vector3(7.5F,0.5f,0), new Vector3(8.5F,0.5f,0), 
				new Vector3(9.5F,0.5f,0), new Vector3(10.5F,0.5f,0), new Vector3(11.5F,0.5f,0)};
            character = new CharacterController[6];
            if (LR == "left") {
				bank = Object.Instantiate (Resources.Load ("Prefabs/Bank", typeof(GameObject)), leftBank, Quaternion.identity, null) as GameObject;
				bank.name = "left";
				this.LR = 0;
			} else {
				bank = Object.Instantiate (Resources.Load ("Prefabs/Bank", typeof(GameObject)), rightBank, Quaternion.identity, null) as GameObject;
				bank.name = "right";
				this.LR = 1;
			}
        }

        public void moveToBank(CharacterController chr) {
            int index = chr.getTag();
            character[index] = chr;
        }
        public Vector3 getPos(int tag) {
            if (bank.name == "left")
                return LchrPosition[tag];
            else 
                return RchrPosition[tag];
        }
        public CharacterController outOfBank(int tag) {
            if (character[tag] != null) {
                CharacterController tmp = character[tag];
                character[tag] = null;
                return tmp;
            }
            else 
                return null;
        }

        public int getLR() {
            return LR;
        }

        public int[] getCount() {
            int[] count = {0, 0};
            for (int i = 0; i < character.Length; i ++) {
                if (character[i] == null) {
                    continue;
                }
                else if (character[i].getMan() == "Devil") {
                    count[0] ++;
                }
                else
                {
                    count[1] ++;
                }
            }
            return count;
        }

        public void init() {
            character = new CharacterController[6];
        }
    }

    public class BoatController{
        GameObject boat;
        // public Move move;
        Vector3 start = new Vector3(-4f, -1.6f, 0);
        Vector3 end = new Vector3(4f, -1.6f, 0);
        Vector3[] seatPos;
        int LR ;
        CharacterController[] seat =  new CharacterController[2];

        public BoatController() {
            LR = 0;
            seatPos = new Vector3[] {new Vector3(-5, -0.3f, 0), new Vector3(-2.8f, -0.3f, 0)};
            
            boat = Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), start, Quaternion.AngleAxis(90, Vector3.forward)) as GameObject;
            // move = boat.AddComponent(typeof(Move)) as Move;
            boat.name = "boat";
            boat.AddComponent(typeof(ClickEvent));

        }

        // 原来动作未分离版本
        // public void Move() {
        //     if (LR == 0) {
        //         move.setDest(end);
        //         LR = 1;
        //     }
        //     else {
        //         move.setDest(start);
        //         LR = 0;
        //     }
        // }
        public Vector3 boatMovePos() {
            LR = LR == 0 ? 1 : 0;
            if (LR == 0) 
                return start;
            else 
                return end;
            
        }
        public Vector3 getSeat() {
            if (seat[0] == null) {
                if (this.getLR() == 1) {
                    Vector3 tmp = seatPos[0] + new Vector3(8, 0, 0);
                    return tmp;
                }
                return seatPos[0];
            }
            else if (seat[1] == null) {
                if (this.getLR() == 1) {
                    Vector3 tmp = seatPos[1] + new Vector3(8, 0, 0);
                    return tmp;
                }
                return seatPos[1]; 
            }
            return new Vector3();
        }
        public bool moveToBoat(CharacterController chr) {
            if (seat[0] == null) {
                seat[0] = chr;
                return true;
            }
            else if (seat[1] == null) {
                seat[1] = chr;
                return true; 
            }
            return false;
        }

        public CharacterController outOfBoat(int tag) {
            if (seat[0] != null && seat[0].getTag() == tag) {
                CharacterController res = seat[0];
                seat[0] = null;
                return res;
            }
            else if (seat[1] != null && seat[1].getTag() == tag) {
                CharacterController res = seat[1];
                seat[1] = null;
                return res;
            }
            return null;
        }
        public GameObject getObj() {
            return boat;
        }
        
        public bool isEmpty() {
            if (seat[0] == null && seat[1] == null) {
                return true;
            }
            return false;
        }
        public bool isFull() {
            if (seat[0] != null && seat[1] != null) {
                return true;
            }
            return false;
        }
        public int getLR() {
            return LR;
        }

        public int[] getCount() {
            int[] count = {0, 0};
            for (int i = 0; i < 2; i ++) {
                if (seat[i] != null && seat[i].getMan() == "Priest") {
                    count[1] ++;
                }
                else if (seat[i] != null && seat[i].getMan() == "Devil") {                    
                    count[0] ++;
                }
            }
            return count;
        }
        public void init() {
            // move.init();
            // if (LR == 1) {
            //     Move();
            // }
            if (LR == 1) boatMovePos();
            boat.transform.position = start;
            seat = new CharacterController[2];
        }
    }

    
}
