using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judger : MonoBehaviour {
    private int score;
    private int miss;
    void Start() {
        score = 0;
        miss = 0;
    }
    public int getScore() {
        return score;
    }
    public bool checkGame() {
        if (miss >= 10) {
            return false;
        }
        return true;
    }

    public void hit(GameObject disk) {
        if(disk.GetComponent<DiskInfo>().color == Color.white) {
            score += 1;
        }else if (disk.GetComponent<DiskInfo>().color == Color.red) {
            score += 2;
        }else {
            score += 3;
        }
    }
    public void Miss() {
        Debug.Log("Miss one!");
        miss += 1;
    }
    public void restart() {
        miss = 0;
        score = 0;

    }
}