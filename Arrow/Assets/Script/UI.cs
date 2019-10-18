using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour {
    Interaction interaction;
    bool flag = true;
    GUIStyle style1;
    GUIStyle style2;
    GUIStyle style3;
    float time = 0;

    private void Start() {
        interaction = Director.getInstance().currentSceneController as Interaction;
        style1 = new GUIStyle("button");
		style1.fontSize = 25;
        style2 = new GUIStyle();
		style2.fontSize = 35;
		style2.alignment = TextAnchor.MiddleLeft;
        style3 = new GUIStyle();
        style3.fontSize = 15;
        style3.alignment = TextAnchor.MiddleLeft;
    }
    
    private void OnGUI()
    {   
        if (interaction.getState() == -1) {
            if (time < 2) {
                time += Time.deltaTime;
                GUI.Label(new Rect(Screen.width/2-70, Screen.height/2-135, 200, 30), "Preparing Arrow...", style2);
            } else {
                GUI.Label(new Rect(Screen.width/2-70, Screen.height/2-135, 200, 30), "Your Score:" + Singleton<Judger>.Instance.getScore().ToString(), style2);
                if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2 - 20, 180, 70), "Play again", style1)) {
                    interaction.reuse();
                    interaction.setState(1);
                    Singleton<Judger>.Instance.restart();
                    time = 0;
                }
            }
        }
        GUI.Label(new Rect(5, 5, 100, 30), "Score: " + Singleton<Judger>.Instance.getScore().ToString(), style3);
        Vector3 wind = interaction.getWind();
        int x = (int)wind.x;
        int y = (int)wind.y;
        string str1, str2, level;
        if (x < 0) 
            str1 = "West";
        else if (x > 0)
            str1 = "East";
        else 
            str1 = "";

        if (y < 0) 
            str2 = "South";
        else if (y > 0)
            str2 = "North";
        else 
            str2 = "";
        
        if (x == 0 && y == 0) {
            str1 = "No wind";
        }
        if (x != 0) {
            int tmp = x > 0 ? x : -x;
            level = tmp.ToString();
        } else if (y != 0) {
            int tmp = y > 0 ? y : -y;
            level = tmp.ToString();
        } else {
            level = "0";
        }

        GUI.Label(new Rect(5, 35, 200, 30), "Wind Direction: " + str1 + str2, style3);
        GUI.Label(new Rect(5, 65, 100, 30), "Wind Level: " + level , style3);
        if (flag) {
            GUI.Label(new Rect(Screen.width/2-60, Screen.height/2-135, 100, 50), "ShootArrow!", style2);
            if(GUI.Button(new Rect(Screen.width/2-70, Screen.height/2 - 20, 150, 70), "Play", style1)) {
                flag = false;
                interaction.setState(1);
            }
        }
        
    }

    private void Update()
    {
        if (interaction.getState() > 0) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (interaction.arrowState() == "ready") {
                interaction.moveArrowDirection(ray.direction);
                if (Input.GetButtonDown("Fire1")) {
                    interaction.shoot(ray.direction);
                }
            }
        }
    }
}