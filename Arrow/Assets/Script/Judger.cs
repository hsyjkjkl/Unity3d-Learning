using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judger : MonoBehaviour{
    private int score;
    void Start() {
        score = 0;
    }
    public int getScore() {
        return score;
    }
    public bool checkGame() {
        if (score == 10) {
            return false;
        }
        return true;
    }

    public void addScore(string str) {
        switch(str) {
            case "t1":
                score += 10;
                break;
            case "t2":
                score += 8;
                break;
            case "t3":
                score += 6;
                break;
            case "t4":
                score += 3;
                break;
            case "t5":
                score += 1;
                break;
        }
    }
    public void restart() {
        score = 0;

    }
}