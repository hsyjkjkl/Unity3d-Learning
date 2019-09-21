# 前言
简答题部分的博客地址：[传送门](https://blog.csdn.net/JKJKL1/article/details/101037982)
资料来源于潘老师的[课程网站](https://pmlpml.github.io/unity3d-learning/)，详细信息可以去了解。
还有附上参考大佬的优秀博客：[传送门](https://www.jianshu.com/p/07028b3da573)
感谢师兄师姐博客的指导！
# 游戏脚本
> Priests and Devils
> \
> Priests and Devils is a puzzle game in which you will help the Priests and Devils to cross the river within the time limit. There are 3 priests and 3 devils at one side of the river. They all want to get to the other side of this river, but there is only one boat and this boat can only carry two persons each time. And there must be one person steering the boat from one side to the other side. In the flash game, you can click on them to move them and click the go button to move the boat to the other direction. If the priests are out numbered by the devils on either side of the river, they get killed and the game is over. You can try it in many > ways. Keep all priests alive! Good luck!

简单翻译和提炼了一下要点：
> 目标：成功将3个牧师和3个魔鬼运到河对岸
> 限制条件：
> 1. 船上最多载2人，且必须至少一人才能开船
> 2. 河的一岸魔鬼人数多于牧师，且牧师数不为0，则游戏失败。
> 
> 用户操作：点击相应的人物使其上下船，点击go开船（由于画面比较小，以下使用了点击船来开船）


# 项目传送门
上传了整个项目到github上：[Github](https://github.com/hsyjkjkl/Unity3d-Learning/tree/master/Devil%20and%20Priest)

# 实现步骤
## 1. 分析游戏对象（Object）
1. 牧师
2. 魔鬼（由于与牧师的动作类似，所以可以归为一类，但是判断逻辑的时候需要特别的编辑来区分）
3. 河岸（南岸、北岸）
4. 船（载人）
5. 河流、光照（装饰？）
## 2. 玩家动作规则表
|玩家动作| 结果 | 条件 |
|--|--|--|
| 点击人物角色（牧师/魔鬼） | 人物上船/下船 | 游戏进行中且人物在岸上/船上 |
| 点击船 | 船开动 | 船上至少有一人且游戏未结束|
| 点击Restart| 重新开始游戏|  游戏结束且弹出Restart的按钮|

## 3. 对象预制制作
用方块建立河流、河岸等对象，调整其合适大小，并且记录其在摄像机范围内显示的坐标。用黑色长条方块表示魔鬼，用白色圆柱体表示牧师，船的话，用一个压扁的胶囊体来替代（有点像冲浪板。。）
大致完成好的场景如下图：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190921200431261.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0pLSktMMQ==,size_16,color_FFFFFF,t_70)
记录位置的参数，以便使用代码生成的时候不许重新调整。

# 代码结构
根据老师课上所说，采用MVC结构，其中Controller是实现逻辑的控制器，既要负责Model的运动，还有游戏逻辑判断，以及用户界面的显示。所以Controller的职责非常多，但是不能将代码全都堆在一个类里面，所以就需要对Controller进行一个分层管理。
## Director
首先是Director，最高级的一层管理，是单例模式，也就是说一个游戏里面只能由一个Director实例。我觉得Director这个比喻非常好呀，无论是什么场景，都只能有一个指手画脚的人，不然就会乱套。而Director就充当了这样一个角色，从名义上他管理所有事务，但实际上他只负责管理底下的Controller，吩咐它们去完成各种不同的功能。代码如下：
```csharp
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
```
单例模式保证了每一个场景中都会得到同一个Director。

## 各类Controller（Model）
管理不同对象的Controller分别完成对象本身的工作，如实例化、位置调整等。准确来讲不算是Controller，应该算入Model部分（没有搞太清楚）。这一部分是实现对象自己的功能和行为，让其达到我们的目的（上下船，传递参数）。每一部分都是相对独立的。
### BoatController
```csharp
public class BoatController{
        GameObject boat;
        public Move move;
        Vector3 start = new Vector3(-4f, -1.6f, 0);
        Vector3 end = new Vector3(4f, -1.6f, 0);
        Vector3[] seatPos;
        int LR ;
        CharacterController[] seat =  new CharacterController[2];

        public BoatController() {
            LR = 0;
            seatPos = new Vector3[] {new Vector3(-5, -0.3f, 0), new Vector3(-2.8f, -0.3f, 0)};
            
            boat = Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), start, Quaternion.AngleAxis(90, Vector3.forward)) as GameObject;
            move = boat.AddComponent(typeof(Move)) as Move;
            boat.name = "boat";
            boat.AddComponent(typeof(ClickEvent));

        }
        public void Move() {
            // 开船
        }
        public Vector3 getSeat() {
            // 获取座位的坐标
        }
        public bool moveToBoat(CharacterController chr) {
            // 将角色加到座位上，也就是记录角色信息，并且让座位不为空
        }

        public CharacterController outOfBoat(int tag) {
           // 下船（其中某个制定的角色），将座位置空
        }
        public GameObject getObj() {
            return boat;
        }
        
        public bool isEmpty() {
            // 是否空座（判断能否开船）
        }
        public bool isFull() {
            // 是否满座（判断是否还能上人）
        }
        public int getLR() {
            return LR;
        }

        public int[] getCount() {
            // 记录乘客数量，分两种角色记录，所以需要用数组
        }
        public void init() {
            // 初始化，重新开始
        }
    }
```
这就是Boat的各种函数，由于具体实现放上来会占很大篇幅，所以就没有贴具体代码，可以到之前的Github链接上看。
首先构造函数最重要的就是定义好各个位置，如船上的位置（人物所占的位置），南北岸停船的坐标位置等。然后就是实例化Boat对象，加载相应的预制到场景中。然后添加运动脚本和点击事件脚本（稍后讲），船最重要的功能就是载客，所以需要记录空座位以及座位上的乘客信息。还要给出不同乘客的数量，以此来判断。代码中有简单的注释，而且这些功能也不复杂所以就不细说。
### BankController
```csharp
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
            // 将角色加入容器中
        }
        public Vector3 getPos(int tag) {
            // 返回对应tag位置的坐标信息
            // 需要判断南岸还是北岸
        }
        public CharacterController outOfBank(int tag) {
            // 离开岸，将对应容器中的角色置空
        }

        public int getLR() {
            return LR;
        }

        public int[] getCount() {
           //二元数组表示牧师多少个，恶魔多少个
        }

        public void init() {
            character = new CharacterController[6];
        }
    }
```
河岸的类也是基本与船相似，需要用一个容器来装各个角色，这里为了简化计算，将每一个角色规定在了同一个位置，也就是不论怎么上下岸，在岸上的位置是固定的，有角色的tag属性决定。所以在构造函数处，就一次性将南岸和北岸的12个位置坐标给定义好，后面直接根据tag，到相应下标找位置坐标就好了。由于岸也是容器，所以上下岸的操作就是将容器（角色类的数组）中的某个位置置空或赋值就好了。岸本身是不能改变的，所以相比船少了一点功能。

### CharacterContorller
角色主要做的事情就是接受点击事件并且移动，为了表示各个角色的不同（包括位置、身份），需要对外开放许多接口来获取他本身的属性信息。
```csharp
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

        public void goMoving(Vector3 position) {
            move.setDest(position);
        }

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
```
首先是根据输入的构造信息，决定是牧师还是魔鬼，并且实例化对应的资源。
其次就是获取各种属性，如对象名、位置（对应tag）、状态（在船上还是岸上）、身份（牧师还是魔鬼）。
最重要的就是移动，这一部分是通过加载Move类来作为一个脚本部件，调用Move中的函数来实现移动。与船的移动是相似的。
### Move 移动脚本类
```csharp
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
```
Move的逻辑不难理解，就是每次Update就更新自己的位置，只要设置了目标位置，使用Vector的towards函数，就能每一次更新的时候移动一定的距离，直到到达目的地为止。
其中移动部分要分为两个过程，一个是从岸边平移到船的上方，另一个是从上空落到船上（上岸则为反向过程），所以需要标记两个状态，每个状态标识一段过程。如果是船的话，只有平移一个过程，所以可以直接跳过第一个状态。对外提供的setDest函数就是设置目的地坐标，使其能够运动到相应的位置。由于是通过Update来运动的，所以当挂载到相应的对象上，只要set一下目的地，就能够运动了。

## 总的Controller
由于刚刚各个控制器都是负责自己部分的工作，所以需要一个总的领导将他们各个功能配合起来，形成一个完整的系统。
```csharp
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
```
这个总类，一开始先把所有对象创建出来，设置相关的属性。然后有几个函数分别对应上船，开船，上岸等操作，其中上船和上岸同属于人物的动作。
根据之前的代码可以知道，上船并不只是人物的简单移动，而且还需要设置岸上的空位，船上空位被填充等等，需要一点条件判断。
而且有一个细节就是在物体移动过程中，不能上下船，所以需要有一个判断禁止动作的函数，根据之前move类可以知道，移动是有状态来标识的，所以只要找到标识，看是否处于移动中，如果是就直接退出函数不执行相应的操作。
至于判断游戏结束条件则很简单了，只需要统计任意一边岸上的牧师与魔鬼数量就好了。这里值得注意的是，需要加上船上的人数，否则会出现因为下船顺序先后输掉游戏的bug。

## ClickEvent
最后就是用户交互的部分了。
```csharp
public class ClickEvent :MonoBehaviour {
    Interaction interaction;
    dpGame.CharacterController character;
    public void setChrController (dpGame.CharacterController chr) {
        character = chr;
    }

    void Start() {
        interaction = Director.getInstance().currentSceneController as Interaction;

    }

    private void OnMouseDown()
    {   
        if (gameObject.name == "boat") {
            interaction.MoveBoat();
        }
        else {
            interaction.moveCharacters(character);
        }
    }
}
```
添加监听点击动作，对应不同物体进行不同操作，如人物上下船、开船。
还有一个游戏结束后的GUI:
```csharp
public class UI : MonoBehaviour {
    private Interaction interaction;
    public int status = 1;
    GUIStyle style1;
	GUIStyle style2;
    void Start() {
		interaction = Director.getInstance ().currentSceneController as Interaction;

		style1 = new GUIStyle();
		style1.fontSize = 40;
		style1.alignment = TextAnchor.MiddleCenter;
        style1.normal.textColor = Color.red;

		style2 = new GUIStyle("button");
		style2.fontSize = 20;
	}
    void OnGUI() {
		if (status == 0) {
            style2.fontSize = 20;
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-95, 100, 50), "Gameover!", style1);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 160, 70), "Play again!", style2)) {
				status = 1;
				interaction.Restart ();
			}
		} else if(status == 2) {
            style2.fontSize = 25;
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-95, 100, 50), "You win!", style1);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 140, 70), "Restart", style2)) {
				status = 1;
				interaction.Restart ();
			}
		}
	}
}
```
游戏截图：
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190921225941503.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0pLSktMMQ==,size_16,color_FFFFFF,t_70)
本次实验到此为止。
