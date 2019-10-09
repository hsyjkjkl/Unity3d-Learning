using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserUI : MonoBehaviour {
    private Interaction action;
    bool flag = true;
    GUIStyle style1;
    GUIStyle style2;
    GUIStyle style3;
    float time = 0;
    void Start ()
    {
        action = Director.getInstance().currentSceneController as Interaction;
        style1 = new GUIStyle("button");
		style1.fontSize = 25;

        style2 = new GUIStyle();
		style2.fontSize = 35;
		style2.alignment = TextAnchor.MiddleCenter;
        style3 = new GUIStyle();
        style3.fontSize = 25;
        style3.alignment = TextAnchor.MiddleCenter;
    }
    private void OnGUI() {
        if (action.getState() == -1) {
            GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-105, 100, 50), "Game Over!", style2);
            GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-55, 110, 40), "Your Score: "+Singleton<Judger>.Instance.getScore().ToString(), style3);
            if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 150, 70), "Play again", style1)){
                Singleton<Judger>.Instance.restart();
                action.reset();
                action.changeState(1);
            }
            return;
        }else if (action.getState() == -2) {
            GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-105, 100, 50), "Finished!", style2);
            GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-55, 110, 40), "Your Score: "+Singleton<Judger>.Instance.getScore().ToString(), style3);
            if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 150, 70), "Restart", style1)){
                Singleton<Judger>.Instance.restart();
                action.reset();
                action.changeState(1);
            }
            return;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 pos = Input.mousePosition;
            action.hit(pos);
        }
        GUI.Label(new Rect(5, 5, 100, 50), "Score: " +Singleton<Judger>.Instance.getScore().ToString(), style3);
 
        if (flag) {
            GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-95, 100, 50), "Hit UFO!", style2);
            if(GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 150, 70), "Play", style1)) {
                flag = false;
                action.changeState(1);
            }
        }
 
        if (!flag && action.getState() == 2)
        {
            GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-95, 100, 50), "Next Round!", style2);
            time += Time.deltaTime;
            if (time > 3.5) {
                action.changeState(3);
                time = 0;
            }
        }
    }
    
}