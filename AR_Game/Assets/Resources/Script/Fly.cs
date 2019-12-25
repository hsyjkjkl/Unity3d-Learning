using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public float speed = 5.5f;
    private Rigidbody rig;
    public Controller controller;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        controller = Director.getInstance().currentSceneController as Controller;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controller == null) {
            controller = Director.getInstance().currentSceneController as Controller;
            return;
        }
        if (controller.getState() == 0) {
            this.transform.localPosition = Vector3.zero;
            rig.useGravity = false;
            return;
        }
        if (this.transform.position.y < -5f) {
            rig.useGravity = false;
            rig.velocity = Vector3.zero;
        }
        else {
            rig.useGravity = true;
        }
        if(Input.GetButtonDown("Fire1")) {
            rig.velocity = Vector3.up * speed;
        }
    }
}
