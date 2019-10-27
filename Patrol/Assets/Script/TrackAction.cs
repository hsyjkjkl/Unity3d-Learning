using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingAction : SSAction
{
    private float speed = 2.5f;          // 跟随玩家的速度
    private GameObject player;           // 玩家
    private PatrolData info;             // 巡逻兵数据

    public static TrackingAction GetAction(GameObject player) {
        TrackingAction action = CreateInstance<TrackingAction>();
        action.player = player;
        return action;
    }

    public override void Start() {
        info = this.gameObject.GetComponent<PatrolData>();
        this.gameObject.GetComponent<Animator>().SetBool("track", true);
    }

    public override void Update() {
        FirstController controller = Director.getInstance().currentSceneController as FirstController;

        if (controller.getState() == 1) {

            transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
            this.transform.LookAt(player.transform.position);
            // if (info.onCollison && !info.withTeammate) {
            //     this.transform.Rotate(Vector3.up, 180);
            //     GameObject tmp = new GameObject();
            //     tmp.transform.position = this.transform.position;
            //     tmp.transform.rotation = this.transform.rotation;
            //     tmp.transform.Translate(0, 0, Random.Range(0.5f, 2f));
            //     float x = tmp.transform.position.x;
            //     float z = tmp.transform.position.z;
            //     this.transform.LookAt(new Vector3(x, 0, z));
            //     Destroy(tmp);

            //     this.destroy = true;    
            //     this.enable = false;
            //     this.callBack.SSActionEvent(this);
            //     this.gameObject.GetComponent<PatrolData>().tracking = false;
            //     this.gameObject.GetComponent<PatrolData>().withTeammate = false;
            //     Singleton<GameEventManager>.Instance.PlayerEscape(this.gameObject);
            // }
            if (info.tracking && (!(info.seePlayer && info.patrolArea == info.playerArea) || (info.onCollison && !info.withTeammate))) {
                this.destroy = true;    
                this.enable = false;
                this.callBack.SSActionEvent(this);
                this.gameObject.GetComponent<PatrolData>().tracking = false;
                this.gameObject.GetComponent<PatrolData>().withTeammate = false;
                Singleton<GameEventManager>.Instance.PlayerEscape(this.gameObject);
            }
        }
    }
}