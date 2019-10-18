using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAction : SSAction{
    public Vector3 force;
    public Vector3 affect;
    public static ArrowAction GetSSAction(Vector3 f, Vector3 wind) {
        ArrowAction action = ScriptableObject.CreateInstance<ArrowAction>();

        action.force = f;
        action.affect = wind;
        return action;
    }

    public override void FixedUpdate() {
        this.gameObject.GetComponent<Rigidbody>().AddForce(affect, ForceMode.Acceleration);

        if (this.transform.position.z > 3 ||  Mathf.Abs(this.transform.position.y) > 7 || 
            Mathf.Abs(this.transform.position.x) > 10 || this.gameObject.tag == "ontarget") {
            this.destroy = true;
            if (this.gameObject.tag != "ontarget")
                this.callBack.SSActionEvent(this);
        }
    }
    public override void Update(){}
    public override void Start() {
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}