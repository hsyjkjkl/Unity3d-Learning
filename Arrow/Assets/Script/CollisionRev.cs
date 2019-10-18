using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionRev : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {   
        
        GameObject arrow = other.gameObject;
        if (arrow.name == "arrow") {
            string str = this.name;
            arrow.GetComponent<Rigidbody>().velocity = Vector3.zero;
            arrow.GetComponent<Rigidbody>().isKinematic = true;

            Singleton<Judger>.Instance.addScore(str);
            arrow.transform.position += Vector3.forward * 0.001f;
            arrow.name = "ontarget";
            Controller controller = Director.getInstance().currentSceneController as Controller;
            controller.hit(arrow);
            // if (controller.arrow == null)
            controller.getArrow();
        }
    }
}