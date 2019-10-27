using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolActionManager : SSActionManager, ISSActionCallback
{
    public PatrolAction patrol;
    public TrackingAction follow;

    // 巡逻
    public void Patrol(GameObject ptrl) {
        this.patrol = PatrolAction.GetAction(ptrl.transform.position);
        this.RunAction(ptrl, patrol, this);
    }

    // 追击
    public void Follow(GameObject player, GameObject patrol) {
        this.follow = TrackingAction.GetAction(player);
        this.RunAction(patrol, follow, this);
    }

    //停止所有动作
    public void DestroyAllActions() {
        DestroyAll();
    }

    public void SSActionEvent(SSAction source){ }
}