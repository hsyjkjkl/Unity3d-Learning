using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public Texture cross;
    public Texture circle;
    public Texture2D white;
    private int[,] array = new int [3, 3];
    private int finish = 0, turn = 1, count = 0; 
    GUIStyle style = new GUIStyle();
    GUIStyle tStyle = new GUIStyle();

    void init() {
        for (int i = 0; i < 3; i ++) {
            for (int j = 0; j < 3; j ++) {
                array[i, j] = 0;
            }
        }
        finish = 0;
        turn = 1;
        count = 0;
    }

    int isFinished()
    {
        for (int i = 0; i < 3; i ++) {
            if (array[i, 0] == array[i, 1] && array[i, 1] == array[i, 2]) {
                return array[i, 0];
            }
        }
        for (int i = 0; i < 3; i ++) {
            if (array[0, i] == array[1, i] && array[1, i] == array[2, i]) {
                return array[0, i];
            }
        }
        if (array[1, 1] == array[0, 0] && array[1, 1] == array[2, 2] ||
            array[1, 1] == array[0, 2] && array[1, 1] == array[2, 0]) {
            return array[1, 1];
        }
        return 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnGUI() {
        style.fontSize = 30;
        style.normal.textColor = Color.red;
        style.normal.background = null;
        tStyle.fontSize = 40;
        tStyle.fontStyle = FontStyle.Bold;
        GUI.Label(new Rect(290, 10, 100, 100), "Tic Tac Toe", tStyle);

        if (GUI.Button (new Rect (150, 220, 100, 50), "reset")) {
            init();
        }
        if (count >= 5 && isFinished() != 0) {
            finish = 1;
            string str = (isFinished() == 1 ? "Player1 Win!" : "Player2 Win!");
            GUI.Label(new Rect(300f, 50f, 200f, 60), str, style);
        } 
        else if (count == 9) {
            finish = 1;
            GUI.Label(new Rect(350f, 50f, 200f, 60), "Draw", style);
        }
        
        style.normal.background = white;
        for (int i = 0; i < 3; i ++) {
            for (int j = 0; j < 3; j ++) {
                if (array[i,j] == 1) {
                    GUI.backgroundColor = Color.white;
                    GUI.Button(new Rect(i*60 + 300f, j *60 + 100f, 60, 60), circle);
                }
                else if (array[i,j] == 2) {
                    GUI.backgroundColor = Color.white;
                    GUI.Button(new Rect(i*60 + 300f, j *60 + 100f, 60, 60), cross);
                }
                else if (GUI.Button(new Rect(i*60 + 300f, j *60 + 100f, 60, 60), white)) {
                    if (finish == 0) {
                        array[i, j]= turn;
                        turn = (turn == 1 ? 2 : 1);
                        count ++;
                    }
                }
            }
        }
    }
}
