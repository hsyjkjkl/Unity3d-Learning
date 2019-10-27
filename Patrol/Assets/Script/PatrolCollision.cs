using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolCollision : MonoBehaviour
{
    public float time = 0;
    void OnCollisionEnter(Collision collision) {
    //     Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Player") {
            this.GetComponent<Animator>().SetTrigger("attack");
            Singleton<GameEventManager>.Instance.OnPlayerCatched();
        } else {
            
            if (collision.gameObject.name != "Plane") {
                
                this.GetComponent<PatrolData>().onCollison = true;
            }
            if (collision.gameObject.name == "Zombie") {
                this.GetComponent<PatrolData>().withTeammate = true;
            }
        }
    }

    void OnCollisionStay(Collision collision) {
        
        if (collision.gameObject.name != "Plane") {
            time += Time.deltaTime;
            if (time > 1.5) {
                this.GetComponent<PatrolData>().onCollison = true;
                time = 0;
            }
        }
    }
}