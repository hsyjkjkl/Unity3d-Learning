using UnityEngine;
using Vuforia;

public class VirtualButtonEventHandler : MonoBehaviour, IVirtualButtonEventHandler {
    public GameObject vb;
    public GameObject ball;

    public float speed = 7f;
    private Rigidbody rig;
    public Controller controller;
    bool isPressed = false;

    void Start() {
        VirtualButtonBehaviour vbb = vb.GetComponent<VirtualButtonBehaviour>();
        if(vbb){
            vbb.RegisterEventHandler(this);
        }
        rig = ball.GetComponent<Rigidbody>();
        controller = Director.getInstance().currentSceneController as Controller;
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb) {
        if (controller == null) {
            controller = Director.getInstance().currentSceneController as Controller;
            return;
        }
        if (controller.getState() == 0) {
            return;
        }
        isPressed = true;
        Debug.Log("Pressed!");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb) {
        isPressed = false;
        Debug.Log("Released!");
    }
    private void FixedUpdate()
    {
        if (isPressed) {
            rig.velocity = Vector3.up * speed;
        }
    }
}