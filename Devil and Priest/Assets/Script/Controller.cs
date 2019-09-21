using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dpGame;

public class Controller : MonoBehaviour, SceneController, Interaction {
    // bool forbid = false;
    public BankController leftBank;
    public BankController rightBank;
    public BoatController boat;
    public dpGame.CharacterController[] characters;
    public Vector3 riverPos = new Vector3(0, -2.5f, 0);
    UI u;

    void Awake() {
		Director director = Director.getInstance ();
		director.currentSceneController = this;
		u = gameObject.AddComponent <UI>() as UI;
		characters = new dpGame.CharacterController[6];
		loadResources ();
	}
    public void loadResources() {
        GameObject river = Instantiate (Resources.Load ("Prefabs/River", typeof(GameObject)), new Vector3(0, -3f, 0), Quaternion.identity, null) as GameObject;
		river.name = "river";

		leftBank = new BankController ("left");
		rightBank = new BankController ("right");
		boat = new BoatController ();

		for (int i = 0; i < 3; i ++) {
            dpGame.CharacterController tmp = new dpGame.CharacterController("devil", i);
            tmp.setPosition(leftBank.getPos(i));
            tmp.moveToBank(leftBank);
            leftBank.moveToBank(tmp);
            characters[i] = tmp;
        }

        for (int i = 0; i < 3; i ++) {
            dpGame.CharacterController tmp = new dpGame.CharacterController("priest", i + 3);
            tmp.setPosition(leftBank.getPos(i+3));
            tmp.moveToBank(leftBank);
            leftBank.moveToBank(tmp);
            characters[i+3] = tmp;
        }

    }

    public void MoveBoat() {
        if (u.status ==0 ) return;
        if (forbidEvent()) return;
        if (! boat.isEmpty()) {
            boat.Move();
        }
        u.status = checkGame();
    }

    public void moveCharacters (dpGame.CharacterController chr) {
        if (u.status ==0 ) return;
        if (forbidEvent()) return;
        if (chr.getState() == 1) {
            BankController bank;
            if (boat.getLR() == 0) {
                bank = leftBank;
            }
            else {
                bank = rightBank;
            }
            boat.outOfBoat(chr.getTag());
            chr.moveToBank(bank);
            chr.goMoving(bank.getPos(chr.getTag()));
            bank.moveToBank(chr);
        }
        else {
            BankController bank = chr.getBank();
            
            if (boat.getLR() == bank.getLR()) {
                if (!boat.isFull()) {
                    bank.outOfBank(chr.getTag());
                    chr.moveToBoat(boat);
                    chr.goMoving(boat.getSeat());
                    boat.moveToBoat(chr);
                    
                }
            }
        }
        u.status = checkGame();
    }

    int checkGame() {
        int leftP = 0, rightP = 0, leftD = 0, rightD = 0;
        int[] LCount = leftBank.getCount();
        leftP += LCount[1];
        leftD += LCount[0];

        int[] RCount = rightBank.getCount();
        rightD += RCount[0];
        rightP += RCount[1];

        int[] Bcount = boat.getCount();
        Debug.Log(Bcount[0] + " " + Bcount[1]);


        if (rightD + rightP == 6)		// win
			return 2;
        
        // 计算上船上的人
        if (boat.getLR() == 0) {
            leftP += Bcount[1];
            leftD += Bcount[0];
        }
        else if (boat.getLR() == 1) {
            rightP += Bcount[1];
            rightD += Bcount[0];
        }

        Debug.Log("LP: " + leftP + " LD: " + leftD); //测试用

        // 如果魔鬼数量大于牧师，并且牧师数量不为0就失败！
        if (leftP < leftD && leftP != 0) {
            return 0;
        }
        if (rightP < rightD && rightP != 0) {
            return 0;
        }
        return 1;
    }

    //检查是否正在运动
    public bool forbidEvent() {
        if (boat.move.state != 0) return true;
        for (int i = 0; i < characters.Length; i ++) {
            if (characters[i].move.state != 0)
                return true;
        }
        return false;
    }
    public void Restart() {
        boat.init();
        leftBank.init();
        rightBank.init();
        for (int i = 0; i < characters.Length; i ++) {
            characters[i].init();
        }
    }
}