using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAction : SSAction {
    private float x, z;
    private bool turn = false;
    private PatrolData info;

    public static PatrolAction GetAction(Vector3 pos) {
        PatrolAction action = CreateInstance<PatrolAction>();
        action.x = pos.x;
        action.z = pos.z;
        return action;
    }

    public override void Start() {
        info = this.gameObject.GetComponent<PatrolData>();
    }

    public override void Update(){
        if (Director.getInstance().currentSceneController.getState() == 1) {
            PatrolWalk();
            if (!info.tracking && info.seePlayer && info.patrolArea == info.playerArea) {
               this.destroy = true;
               this.enable = false;
               this.callBack.SSActionEvent(this);
               this.gameObject.GetComponent<PatrolData>().tracking = true;
               Singleton<GameEventManager>.Instance.FollowPlayer(this.gameObject); 
            }
        }
    }

    void PatrolWalk() {
        if (turn) {
            x = this.transform.position.x + Random.Range(-7f, 7f);
            z = this.transform.position.z + Random.Range(-7f, 7f);
            this.transform.LookAt(new Vector3(x, 0, z));
            this.gameObject.GetComponent<PatrolData>().onCollison = false;
            turn = false;
        }
        float distance = Vector3.Distance(transform.position, new Vector3(x, 0, z));

        if (this.gameObject.GetComponent<PatrolData>().onCollison) {
            float angle = Random.Range(175, 185);
            this.transform.Rotate(Vector3.up, angle);
            GameObject tmp = new GameObject();
            tmp.transform.position = this.transform.position;
            tmp.transform.rotation = this.transform.rotation;
            tmp.transform.Translate(0, 0, Random.Range(0.5f, 2f));
            x = tmp.transform.position.x;
            z = tmp.transform.position.z;
            this.transform.LookAt(new Vector3(x, 0, z));
            this.gameObject.GetComponent<PatrolData>().onCollison = false;
            Destroy(tmp);
        } else if (distance <= 0.1) {
            turn = true;
        } else {
            this.transform.Translate(0, 0, Time.deltaTime );
        }
    } 
}