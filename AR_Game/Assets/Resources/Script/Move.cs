using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Controller controller;
    public float speed = 9;
    Vector3 target;
    float time = 0;
    bool flag = true;
    // Start is called before the first frame update
    void Start()
    {
        GameObject tmp = GameObject.Find("Bird");
        target = 3*tmp.transform.position - 2*this.transform.position;
        controller = Director.getInstance().currentSceneController as Controller;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.getState() == 0) return;
        if (time > 5) {
            controller.addScore();
            flag = false;
            Debug.Log("score: " + controller.getScore());
            Destroy(this.gameObject);
            return;
        }
        time += Time.deltaTime;

        this.transform.position = Vector3.MoveTowards(transform.position,target,Time.deltaTime * speed);
        // this.transform.Translate(target*speed*Time.deltaTime);
        
    }
}
