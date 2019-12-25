using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCollision : MonoBehaviour
{

    private void OnCollisionEnter(Collision other)
    {
        Controller controller = Director.getInstance().currentSceneController as Controller;
        controller.state = 0;
        Destroy(other.gameObject);
    }
}