using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    public Interaction interaction;
    public SceneController controller;
    bool flag = true;
    GUIStyle style1;
    GUIStyle style2;
    private void Start()
    {
        style1 = new GUIStyle("button");
		style1.fontSize = 25;
        style2 = new GUIStyle();
		style2.fontSize = 35;
        style2.normal.textColor = Color.white;
		style2.alignment = TextAnchor.MiddleCenter;
        interaction = Director.getInstance().currentSceneController as Interaction;
        controller = Director.getInstance().currentSceneController as SceneController;
    }

    private void Update()
    {
        if (controller.getState() == 1) {
            // 获取键盘输入
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            interaction.movePlayer(new Vector3(x,0,z));

            if (Input.GetButtonDown("Fire1")) {
                    interaction.PlayerPick();
            }

        } 
    }

    private void OnGUI()
    {   
        if (flag) {
            GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-135, 100, 50), "Zombie Attack!", style2);
            if(GUI.Button(new Rect(Screen.width/2-75, Screen.height/2 - 20, 150, 70), "Play", style1)) {
                flag = false;
                controller.changeState(1);
            }
        }
        else if (controller.getState() == 2) {
            GUI.Label(new Rect(Screen.width/2-60, Screen.height/2-135, 120, 40), "Finish!\nScore:" + Singleton<Judger>.Instance.getScore().ToString(), style2);
            if (GUI.Button(new Rect(Screen.width/2-90, Screen.height/2 - 20, 180, 70), "Play again", style1)) {
                interaction.reset();
                controller.changeState(1);
                Singleton<Judger>.Instance.restart();
            }
        } else if (controller.getState() == 0) {
            GUI.Label(new Rect(Screen.width/2-60, Screen.height/2-135, 120, 30), "You Lose!", style2);
            if (GUI.Button(new Rect(Screen.width/2-90, Screen.height/2 - 20, 180, 70), "Play again", style1)) {
                interaction.reset();
                controller.changeState(1);
                Singleton<Judger>.Instance.restart();
            }
        }
        
    }
}