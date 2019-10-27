using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCollision : MonoBehaviour {
    public int areaNum = 0;
    public FirstController controller;
    private void Awake() {
        
    }
    void OnTriggerEnter(Collider collider) {
        controller = Director.getInstance().currentSceneController as FirstController;
        if (collider.gameObject.transform.parent != null && collider.gameObject.transform.parent.tag == "Player") {
            controller.playerArea = areaNum;
            
        }
    }
    private void OnTriggerStay(Collider collider)
    {
        if (areaNum == 9) {
            if (collider.gameObject.tag == "Player") {
                controller.playerArea = areaNum;  
                Singleton<GameEventManager>.Instance.Finished();
            }
            
        }
    }
    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Patrol") {
            collider.gameObject.GetComponent<PatrolData>().onCollison = true;
            
        } else if (collider.gameObject.transform.parent != null && collider.gameObject.transform.parent.tag == "Player") {
            controller = Director.getInstance().currentSceneController as FirstController;
            controller.playerArea = 0;
        }
    }
}