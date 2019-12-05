using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AI {
    public static List<State> closed = new List<State>();
    public static State end = new State(0, 0, true);
    public bool isFind = false;

    public bool DFS(ref State root) {
        closed.Add(root);
        if (root.isEqual(end)) {
            isFind = true;
        }
        for (int i = 0; i < 5; i ++) {
            State next = nextState(root, i);
            
            if (next != null) {
                if (closed.Contains(next))
                    continue;

                next.parent = root;
                if (isFind) {
                    next.best_way = root;
                }
                else {
                    closed.Remove(root);
                    root.best_way = next;
                    closed.Add(root);
                }
                
                DFS(ref next);
            }
            
        }
        if (!root.isEqual(end) && root.best_way == null) {
            root.best_way = root.parent;
        }
        return isFind;
    }

    public void print() {
        for (int i = 0; i < closed.Count; i ++) {
            Debug.Log(closed[i].ToString());
        }
    }
    public static bool isValid(State s) {
        if (s.priest != 0 && s.priest < s.devil) { // 左边有牧师且 牧师人数不应少于魔鬼
            return false;
        }
        if (s.priest != 3 && (3-s.priest) < (3-s.devil)) { //右边有牧师且 牧师人数不应少于魔鬼
            return false;
        }
        return true;
    }

    public State nextState(State s, int operation) {
        int p, d;
        bool b;
        p = s.priest;
        d = s.devil;
        b = s.boat;
        State next = null;
        if (b) { // 船在右方
            if (operation == 0) {
                if (3-p >= 1) { // 右方牧师大于1人，可过
                    next = new State(p+1, d, !b);
                }
                else {
                    return null;
                }
            } 
            else if (operation == 1) {
                if (3-p >= 2) { // 右方牧师大于2人，可过
                    next = new State(p+2, d, !b);
                }
                else {
                    return null;
                }
            }
            else if (operation == 2) {
                if (3-d >= 1) { // 右方魔鬼大于1人，可过
                    next = new State(p, d+1, !b);
                }
                else {
                    return null;
                }
            }
            else if (operation == 3) {
                if (3-d >= 2) { // 右方魔鬼大于1人，可过
                    next = new State(p, d+2, !b);
                }
                else {
                    return null;
                }
            }
            else if (operation == 4) {
                if(3-p >= 1 && 3-d >= 1) {
                    next = new State(p+1, d+1, !b);
                }
                else {
                    return null;
                }
            }
        }
        else { // 船在左方
           if (operation == 0) {
                if (p >= 1) {
                    next = new State(p-1, d, !b);
                }
                else {
                    return null;
                }
            } 
            else if (operation == 1) {
                if (p >= 2) {
                    next = new State(p-2, d, !b);
                }
                else {
                    return null;
                }
            }
            else if (operation == 2) {
                if (d >= 1) {
                    next = new State(p, d-1, !b);
                }
                else {
                    return null;
                }
            }
            else if (operation == 3) {
                if (d >= 2) {
                    next = new State(p, d-2, !b);
                }
                else {
                    return null;
                }
            }
            else if (operation == 4) {
                if (p >= 1 && d >= 1) {
                    next = new State(p-1, d-1, !b);
                }
                else {
                    return null;
                }
            } 
        }

        if (isValid(next)) {
            return next;
        }

        return null;
    }
}