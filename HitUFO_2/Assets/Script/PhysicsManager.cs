using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : ActionManager, ISSActionCallback, IActionManager {

    PhysicsAction action;
    Controller controller;
    private void Start()
    {
        controller = Director.getInstance().currentSceneController as Controller;
        controller.actionManager = this;
    }

    public void flyUFO(GameObject disk, Vector3 target, float speed) {
        action = PhysicsAction.GetSSAction(target, speed);
        if (disk.GetComponent<Rigidbody>() == null) {
            disk.AddComponent<Rigidbody>();
            disk.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX|RigidbodyConstraints.FreezeRotationZ;
        }
        this.RunAction(disk, action, this);
    }
     public void SSActionEvent(SSAction action){
        Singleton<DiskFactory>.Instance.freeDisk(action.gameObject);
        controller.used --;
    }
}