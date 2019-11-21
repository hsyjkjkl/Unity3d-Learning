using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Slider slider;
    float factor = 0.1f;
    public bool active = true;
    private int turn = 1;
    private void Start()
    {
    }
	
    void change() {
        if (turn == 1)
            slider.value -= 1f;
        else 
            slider.value += 1f;
        if (slider.value <= 0) {
            slider.value = 0;
            turn = 2;
        }
        if (slider.value >= 100) {
            slider.value = 100;
            turn = 1;
        }
    }
    void Update () {
		this.transform.LookAt (Camera.main.transform.position);
        if (active)
            change();
        Color current = slider.fillRect.transform.GetComponent<Image>().color;
        if (slider.value <= 20) {
            slider.fillRect.transform.GetComponent<Image>().color = Color.Lerp(current, Color.red, factor);
        }
        else if (slider.value <= 70) {
            slider.fillRect.transform.GetComponent<Image>().color = Color.Lerp(current, Color.yellow, factor);
        }
        else {
            slider.fillRect.transform.GetComponent<Image>().color = Color.Lerp(current, Color.green, factor);
        }
	}
}
