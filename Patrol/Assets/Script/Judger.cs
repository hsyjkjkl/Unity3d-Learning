using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judger : MonoBehaviour {
    int score = 0;

    public void addScore() {
        score += 1;
    }
    public int getScore() {
        return score;
    }
    public void restart() {
        score = 0;
    }
}