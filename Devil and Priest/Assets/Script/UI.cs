using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dpGame;

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

		style2 = new GUIStyle("button");
		style2.fontSize = 30;
	}
    void OnGUI() {
		if (status == 0) {
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-85, 100, 50), "Gameover!", style1);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 140, 70), "Restart", style2)) {
				status = 1;
				interaction.Restart ();
			}
		} else if(status == 2) {
			GUI.Label(new Rect(Screen.width/2-50, Screen.height/2-85, 100, 50), "You win!", style1);
			if (GUI.Button(new Rect(Screen.width/2-70, Screen.height/2, 140, 70), "Restart", style2)) {
				status = 1;
				interaction.Restart ();
			}
		}
	}
}