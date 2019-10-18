using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAction : SSAction{
    public float g = 20f;
    public Vector3 to;//初速度方向
    public float v; //初速度
    public float v_down = 0;
    public float time;
    private FlyAction() {}

    public static FlyAction GetSSAction(Vector3 target, float speed) {
        FlyAction action = ScriptableObject.CreateInstance<FlyAction>();
        action.to = target;
        action.v = speed * 1.5f;
        // action.v_down = - action.v * target.y;
        return action;
    }

    public override void Update() {
        time += Time.fixedDeltaTime;
        
        this.transform.position += Vector3.down * (float)(v_down*Time.fixedDeltaTime+0.5*g*(Time.fixedDeltaTime)*(Time.fixedDeltaTime));
        this.transform.position += to * v * Time.fixedDeltaTime;
        v_down = time * g;

        if (this.transform.position.y <= -5) {
            this.destroy = true;
            if (this.transform.position.y > -15) {
                Singleton<Judger>.Instance.Miss();
            }
            this.callBack.SSActionEvent(this);
            
        }
    }
    public override void Start() {}
    public override void FixedUpdate(){}
}