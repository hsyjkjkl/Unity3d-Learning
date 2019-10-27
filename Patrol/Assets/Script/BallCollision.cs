using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour {
    float time = 0;
    bool start = false;
    
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            Debug.Log("Click to pick it!");
            time = 0; 
            start = false;
            // if (Input.GetButtonDown("Fire1")) {
            //     Destroy(this.gameObject);
            // }
        }
            
    }

    void OnCollisionStay(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            if (start == true) time += Time.deltaTime;
            if (Input.GetButtonDown("Fire1")) {
                start = true;
            }
            if (start == true && time > 0.6) {
                time = 0;
                start = false;
                FirstController controller = Director.getInstance().currentSceneController as FirstController;
                controller.addBall();
                Destroy(this.gameObject);
            }
        }
    }
}