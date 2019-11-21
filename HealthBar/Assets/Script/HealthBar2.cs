using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar2 : MonoBehaviour {

    void Start()
    {
        if (Canvas == null) Debug.LogError("Please set a canvas for health bar.");
        m_SliderRectTransform = m_Slider.GetComponent<Slider>().transform as RectTransform;
        
        transform.SetParent(Canvas.transform);

        m_ObjectHeight = FollowTarget.GetComponent<MeshRenderer>().bounds.size.y;
        Debug.Log("object height." + m_ObjectHeight);
    }

    void Update()
    {
        UpdateHealthBarPosition();
        if (active)
            change();
        Color current = m_Slider.fillRect.transform.GetComponent<Image>().color;
        if (m_Slider.value <= 20) {
            m_Slider.fillRect.transform.GetComponent<Image>().color = Color.Lerp(current, Color.red, factor);
        }
        else if (m_Slider.value <= 70) {
            m_Slider.fillRect.transform.GetComponent<Image>().color = Color.Lerp(current, Color.yellow, factor);
        }
        else {
            m_Slider.fillRect.transform.GetComponent<Image>().color = Color.Lerp(current, Color.green, factor);
        }
    }

    void change() {
        if (turn == 1)
            m_Slider.value -= 1f;
        else 
            m_Slider.value += 1f;
        if (m_Slider.value <= 0) {
            m_Slider.value = 0;
            turn = 2;
        }
        if (m_Slider.value >= 100) {
            m_Slider.value = 100;
            turn = 1;
        }
    }
    void UpdateHealthBarPosition()
    {
        Vector3 worldPosition = new Vector3(FollowTarget.transform.position.x, 
            FollowTarget.transform.position.y + m_ObjectHeight, FollowTarget.transform.position.z);

        Vector2 position = Camera.main.WorldToScreenPoint(worldPosition);
        m_SliderRectTransform.position = position;
    }

    public Canvas Canvas;
    public GameObject FollowTarget; // 血条跟踪的对象
    private RectTransform m_SliderRectTransform;
    public Slider m_Slider;
    private float m_ObjectHeight;
    float factor = 0.1f;
    public bool active = true;
    private int turn = 1;
}
