using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowActionManager : SSActionManager, ISSActionCallback {

    ArrowAction arrowAction;
    Controller controller;

    private void Start()
    {
        controller = Director.getInstance().currentSceneController as Controller;
        controller.actionManager = this;
    }

    public void arrowFly(GameObject arrow, Vector3 target, Vector3 wind) {
        arrowAction = ArrowAction.GetSSAction(target, wind);
        if (arrow.GetComponent<Rigidbody>() == null)
            arrow.AddComponent<Rigidbody>();
        else 
            arrow.GetComponent<Rigidbody>().isKinematic = false;
        this.RunAction(arrow, arrowAction, this);
    }

    public void SSActionEvent(SSAction action){
        Singleton<ArrowFactory>.Instance.freeArrow(action.gameObject);
        if (controller.arrow.name == "arrow")
            controller.getArrow();
    }
}