using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyActionManager : ActionManager, ISSActionCallback
{
    FlyAction UFOAction;
    Controller controller;

    private void Start()
    {
        controller = Director.getInstance().currentSceneController as Controller;
        controller.actionManager = this;
    }

    public void flyUFO(GameObject disk, Vector3 target, float speed) {
        UFOAction = FlyAction.GetSSAction(target, speed);
        this.RunAction(disk, UFOAction, this);
    }

    public void SSActionEvent(SSAction action){
        Singleton<DiskFactory>.Instance.freeDisk(action.gameObject);
    }
}