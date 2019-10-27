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