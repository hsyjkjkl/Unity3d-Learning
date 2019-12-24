using UnityEngine;
using Vuforia;

public class VirtualButtonEventHandler : MonoBehaviour, IVirtualButtonEventHandler {
    public GameObject vb;
    public Animator ani;

    void Start() {
        VirtualButtonBehaviour vbb = vb.GetComponent<VirtualButtonBehaviour>();
        if(vbb){
            vbb.RegisterEventHandler(this);
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb) {
        ani.SetTrigger("Take Off");
        ani.SetBool("onPress", true);
        Debug.Log("Pressed!");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb) {
        ani.SetTrigger("Land");
        ani.SetBool("onPress", false);
        Debug.Log("Released!");
    }
}