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