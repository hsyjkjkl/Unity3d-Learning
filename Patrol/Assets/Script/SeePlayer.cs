using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeePlayer : MonoBehaviour {
    public FirstController controller;
    void OnTriggerEnter(Collider collider) {
        controller = Director.getInstance().currentSceneController as FirstController;
        if (collider.gameObject.tag == "Player") {
            
            this.gameObject.transform.parent.GetComponent<PatrolData>().seePlayer = true;
            this.gameObject.transform.parent.GetComponent<PatrolData>().player = collider.gameObject;
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Player") {
            this.gameObject.transform.parent.GetComponent<Animator>().SetBool("track", false);
            this.gameObject.transform.parent.GetComponent<PatrolData>().seePlayer = false;
            this.gameObject.transform.parent.GetComponent<PatrolData>().player = null;
        }
    }
}