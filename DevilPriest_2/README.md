
# 前言

本次项目Github地址：[传送门](https://github.com/hsyjkjkl/Unity3d-Learning/tree/master/DevilPriest_2)

项目的详细内容见潘老师的课程网站：[网站链接](https://pmlpml.github.io/unity3d-learning/04-gameobject-and-graphics)

基本操作演练（构建游戏场景）的实验内容在文章末尾，[点此跳转](#基本操作演练)

# 魔鬼与牧师游戏回顾
在上一个实验中（[上一个实验的博客地址](https://blog.csdn.net/JKJKL1/article/details/101112596)），我们利用基础的MVC结构，来实现了魔鬼与牧师游戏的程序设计，下面来简单回顾一下：
## Model
其中Model负责的是各个游戏对象的属性和基本行为，包括人物角色（魔鬼、牧师），船，以及河的两岸。船和岸是作为一个容器，需要有容纳人物的位置，也需要记录每个位置对应的xyz坐标。而它们都需要有一定的方法去返回自身的信息，比如在左还是右、是第几个人物、返回对象类型的方法等等。
## View
这个就是与用户交互的接口，其中需要接受来自用户的点击信息，并且根据点击的物体不同而传递不同的信息。还有就是反馈，游戏开始或结束需要反馈相应的信息给用户，也就是一个简单的界面。
## Controller
控制器需要将M和V连接器来，达到控制全局的目的，不仅需要从model中获取相应的信息，并且利用这些信息判断他们的位置，还需要从View中获取相应的用户输入，进行相应的物体移动，在Model和View之间充当桥梁的作用。
同时还需要判断游戏进行程度，也就是判断输赢，并且将输赢信息返回给View，从而达到反馈的目的。

# 动作分离基本思路
从以上信息可以看出，Controller承担的责任实在是太多了，所以有必要给它分配几个手下，帮助他工作。那应该怎么分配工作呢，就先从“动作”开始吧。
## 动作基类
首先我们从之前学习了解到物体运动（也就是所说的动作），无非就是物体空间属性的改变嘛，我们接触到的有三种：**平移、旋转、缩放**。

那就给它们定义一个类就好了，可是又不能把它定死呀，万一要用到一个类的时候还要去区分是三种动作的哪一种，那不是很麻烦吗？所以就需要抽象出来一个动作的基类：
```csharp
public class SSAction : ScriptableObject {

    public bool enable = true;
    public bool destroy = false;

    public GameObject gameObject{get;set;}
    public Transform transform{get;set;}
    public ISSActionCallback callBack{get;set;}

    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}
```
该动作类只是一个抽象的类，并没有分具体的平移还是旋转，具体区分需要子类的实现。但是有一个问题，如果直接调用了父类怎么办？毕竟父类可是什么都没有的呀！所以这里实现了虚函数，如果没有子类重写，就会抛出异常。

这样一来，调用动作的时候只需要统一视为基类，不用分他究竟是哪一种动作，让他自己搞定。就相当于对它说，给我做个动作，它就会做自己定义好的那一个动作，不需要你去区分哪一种。

## 简单动作子类
本次项目中只涉及到平移，没有另外两种动作，所以只需实现一个类。
```csharp
public class SSMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    private SSMoveToAction() { }
    public static SSMoveToAction GetSSAction(Vector3 target, float speed)
    {
        SSMoveToAction action = ScriptableObject.CreateInstance<SSMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        if (this.transform.position == target)
        {
            this.destroy = true;
            this.callBack.SSActionEvent(this);
        }
    }

    public override void Start()
    {
        
    }
}
```
这个类主要完成的任务就是将物体以一定的速度平移到目标位置，然后将自己标记为可销毁（运动完就没啥事了），还需要调用回调函数去通知信息（具体通知什么信息得看是谁调用它的，因为这只是一个简单的类，主要负责运动）。

## 组合动作类
为什么有了简单动作不够？还需要一个组合动作？不是说这个项目只有平移吗？

平移动作也是需要不同组合的，比如说走迷宫，总不能划一条直线直接到终点吧？需要一小段一小段的平移组合起来，绕过障碍物才能到达终点。所以组合动作就是负责这些不同种类（或者相同种类）的许多小的动作组合起来形成的。

在本次项目中，人物的位置比船上的位置要高（y坐标沿正方向距离比较远），所以如果直接一段平移的话，就会出现穿过河岸直接到船上的情况，所以这个动作需要分解为两次平移，先平移到船的上方，再落下到船上。
```csharp
public class CCSequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence; 
    public int repeat = -1;
    public int start = 0;

    public static CCSequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence)
    {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (start < sequence.Count)
        {
            sequence[start].Update();
        }
    }

    public void SSActionEvent(SSAction action)
    {
        action.destroy = false;
        this.start++;
        if (this.start >= sequence.Count)
        {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0)
            {
                this.destroy = true;
                this.callBack.SSActionEvent(this);
            }
        }
    }

    public override void Start()
    {
        foreach (SSAction action in sequence)
        {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callBack = this;
            action.Start();
        }
    }

    void OnDestroy()
    {
        foreach (SSAction action in sequence)
        {
            Destroy(action);
        }
    }
}
```
很直观地看到，这个类就是创建一个列表来放各个简单动作。

start阶段把每个简单动作的callback函数设为自己，那么一个简单动作完成之后，就会调用在这个类里面使用的通知函数。

Update阶段依次调用各个简单动作的Update函数，使其顺序运动。

每个简单动作完成之后，就会调用回调函数，此时就会执行`SSActionEvent`查看这个动作有没有重复的属性，没有则直接删除并且跳到下一个动作执行。

## 事件通知接口
需要用到回调事件的都需要实现这个接口。
```csharp
public interface ISSActionCallback
{
    void SSActionEvent(SSAction action);
}
```

## 动作管理基类
好了，下面就是该考虑如何调用动作的问题。

现在根据已有的类，我们调用的方法就是新建简单动作，然后根据需要构建组合动作，再调用每个动作的Update函数。。。这不就又回到了一开始的状态吗？每个动作都需要Controller自己来做，而且还需要放到Update函数使得动作的Update能够正常运行，这听起来就很不现实，Controller都没有Update函数，他只是逻辑判断的呀。仅仅将动作抽象为一个类，然后再去调用，这不是跟上一版的实现一样嘛（只不过加上组合动作的类而已），这一点都不面向对象。

所以动作管理的类出现了，看到“管理”两个字，就知道是把许多动作整合到一起，即使是不同物体的不同动作，都统一放到一起，再顺序调用。回看刚刚的组合动作将简单动作整合到一起，这不就像一层一层的管理体系吗？
```csharp
public class SSActionManager : MonoBehaviour
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingForAdd = new List<SSAction>();
    private List<int> waitingForDelete = new List<int>();

    protected void Update()
    {
        foreach (SSAction action in waitingForAdd)
        {
            actions[action.GetInstanceID()] = action;
        }
        
        waitingForAdd.Clear();

        foreach (KeyValuePair<int,SSAction> pair in actions)
        {
            SSAction action = pair.Value;
            if (action.destroy)
            {
                waitingForDelete.Add(action.GetInstanceID());
            } else if (action.enable)
            {
                action.Update();
            }
        }

        foreach (int key in waitingForDelete)
        {
            SSAction action = actions[key];
            actions.Remove(key);
            Destroy(action);
        }
        
        waitingForDelete.Clear();
    }

    public void RunAction(GameObject gameObject, SSAction action, ISSActionCallback callback)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callBack = callback;
        
        waitingForAdd.Add(action);
        action.Start();
    }
}
```
这个类就是将更多的动作管理到一起，构建等待添加和等待删除的队列。

Update函数里，将每个等待添加的动作加入到字典中去。开始执行。

对外提供了一个接口RunAction，让外界可以添加动作。

## 魔鬼与牧师中的动作管理类
既然上面的是基类，那肯定有子类是吧。子类的作用很简单，就是将船移动和人物移动的动作包装起来，使得Controller只需要调用一个方法，就能实现对应物体的运动。
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dpGame;
public class ScenceActionManager : SSActionManager, ISSActionCallback {
    SSMoveToAction boatAction;
    CCSequenceAction characterAction;
    Controller controller;
    Judger judger;
    private void Start()
    {
        controller = Director.getInstance().currentSceneController as Controller;
        controller.actionManager = this;
        judger = controller.judger;
    }

    public void moveBoat(GameObject boat, Vector3 pos, float speed) {
        judger.setForbid(true);
        boatAction = SSMoveToAction.GetSSAction(pos, speed);
        Debug.Log("Ready to run!");
        this.RunAction(boat, boatAction, this);
    }
    public void moveCharacter(GameObject chr, Vector3 pos, float speed) {
        judger.setForbid(true);
        Vector3 start = chr.transform.position;
        Vector3 tmp = pos;
        if (start.y > pos.y) {
            tmp.y = start.y;
        }
        else if (start.y < pos.y) {
            tmp.x = start.x;
        }
        SSAction act1 = SSMoveToAction.GetSSAction(tmp, speed);
        SSAction act2 = SSMoveToAction.GetSSAction(pos, speed);
        characterAction = CCSequenceAction.GetSSAcition(1, 0, new List<SSAction>{act1, act2});
        this.RunAction(chr, characterAction, this);
    }
    public void SSActionEvent(SSAction action) {
        judger.setForbid(false);
    }
}
```
实际上就是用两个函数分别实现船的简单运动，以及人物的组合运动，然后调用`RunAction`函数，使得Controller能够直接调用，一步到位。

# 裁判类（2019新添加）
负责动作的已经弄好了，那再用个裁判类吧，顾名思义，裁判类就负责监管是否存在违背规则的行为，并且反馈信息给Controller。规则具体有以下几点：（代表着裁判类要实现的功能）
> 游戏输赢：
> 1. 成功将3个牧师和3个魔鬼运到河对岸，则游戏胜利。
> 2. 河的一岸魔鬼人数多于牧师，且牧师数不为0，则游戏失败。
> 
> 游戏限制：
> 1. 物体移动过程中不能移动其他物体

按照以上规则，将原来判断游戏输赢的逻辑放到Judger里面就可以了。
（其实更严谨的做法是将Controller里面的变量设置为私有的，然后通过get、set函数来获取和更改，这里因为懒直接写为了公共变量，便于直接访问）
至于禁止移动的逻辑，则需要首先在Controller里面设置一个状态变量，依次来判断当前是否能移动。然后通过Judger来改变这个变量。由于移动是在运动管理的类里面，所以需要在运动开始前将设置变量，使其它物体不能移动，然后在回调函数里（运动结束后），重新设置回去，就是实现了这一个规则。
```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dpGame;

public class Judger : MonoBehaviour {
    public int checkGame()
    {
        Controller controller = Director.getInstance().currentSceneController as Controller;
        int leftP = 0, rightP = 0, leftD = 0, rightD = 0;
        int[] LCount = controller.leftBank.getCount();
        leftP += LCount[1];
        leftD += LCount[0];

        int[] RCount = controller.rightBank.getCount();
        rightD += RCount[0];
        rightP += RCount[1];

        int[] Bcount = controller.boat.getCount();
        Debug.Log(Bcount[0] + " " + Bcount[1]);


        if (rightD + rightP == 6)		// win
			return 2;
        
        // 计算上船上的人
        if (controller.boat.getLR() == 0) {
            leftP += Bcount[1];
            leftD += Bcount[0];
        }
        else if (controller.boat.getLR() == 1) {
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

    public void setForbid(bool b) {
        Controller controller = Director.getInstance().currentSceneController as Controller;
        controller.forbid = b;
    } 
}
```

（补充：其实还有关于船上有人才能开之类的规则，都可以添加到裁判类里面去）

# 前后版本对比
在前一版中运动是通过挂载一个Move的脚本类，将这个Move作为Script部件加到实例化的对象中去。
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
在每个人物中设置Move的方法，也就是调用Move脚本中的setPos方法，使得目标点坐标改变，然后随着Update的更新而改变坐标。在Controller中，就调用物体对象本身的运动方法。

这种实现一个很大的弊端就是运动的模式是比较固定的，因为是根据本次项目来进行编写的，所以无法进行自由的组合。也就是说假如要改变动作，就需要重写这个方法。

新的一版中，各个Model类里面不需要再实现运动的方法，直接通过运动类来改变相应对象的坐标。而且动作可以自由组合，可以实现更多复杂的动作（虽然在本项目中没有）

# 基本操作演练
## 1.导入材料包
在Unity的Asset Store中找到Fantasy Skybox FREE的材料包，然后下载并且导入自己的项目中。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20190928143323113.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0pLSktMMQ==,size_16,color_FFFFFF,t_70)

导入成功后，可以看到文件目录结构变成了以下样子：

![在这里插入图片描述](https://img-blog.csdnimg.cn/20190928143522284.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0pLSktMMQ==,size_16,color_FFFFFF,t_70)

可以看到里面有许多素材。

## 2.地形创建

新建地形对象（GameObject-->3D Object -->Terrain），调整相应的位置使其铺满视野：

![在这里插入图片描述](https://img-blog.csdnimg.cn/20190928143754829.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0pLSktMMQ==,size_16,color_FFFFFF,t_70)

接下来就可以构建山和树了，首先选取修改地形的工具，选择下拉框中的提升和降低地形：

![在这里插入图片描述](https://img-blog.csdnimg.cn/20190928145850968.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0pLSktMMQ==,size_16,color_FFFFFF,t_70)

在场景里面点击，就可以构建山脉和谷地了。（Shift+点击是降低地形）

还可以选择平滑等工具，使得山更加自然。

然后选择绘制地形，就可以使用相应的图层给地形上色：

![在这里插入图片描述](https://img-blog.csdnimg.cn/20190928150135417.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0pLSktMMQ==,size_16,color_FFFFFF,t_70)

上色完成之后，可以选择种树，添加树的材料然后“种”到地形上：

![在这里插入图片描述](https://img-blog.csdnimg.cn/20190928150606676.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0pLSktMMQ==,size_16,color_FFFFFF,t_70)

在摄像机添加天空盒部件，成果：

![在这里插入图片描述](https://img-blog.csdnimg.cn/2019092815125374.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L0pLSktMMQ==,size_16,color_FFFFFF,t_70)

## 3.简单总结游戏对象使用

常用的游戏对象就是3dObject 里面的物体了，方块、球、胶囊体、圆柱体等，常用的就是给他挂载一个部件使其运动。

比较特殊的对象就是我们刚刚利用到的地形，可以改变局部地形高度，颜色等使其产生出山、谷底等形状，还能通过模型构建植被。

游戏对象可以添加各种组建，默认的组件有Transform等，还可以添加轨迹等组件使得对象的属性更加丰富。尤其是添加脚本类，可以使得对象按一定逻辑运行（不一定是本对象）。
