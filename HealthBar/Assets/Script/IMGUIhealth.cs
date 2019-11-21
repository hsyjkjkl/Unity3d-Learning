using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMGUIhealth : MonoBehaviour
{
    public float value;
    public float pos;
    private float tmp;
    
    // Start is called before the first frame update
    private void OnGUI()
    {
        if (GUI.Button(new Rect(450, 50, 40, 40), "+"))
        {
            tmp += 10;
            if (tmp > 100)
                tmp = 100;
        }

        if (GUI.Button(new Rect(100, 50, 40, 40), "-"))
        {
            tmp -= 10;
            if (tmp < 0)
                tmp = 0;
        }

        value = Mathf.Lerp(value, tmp, 0.05f);

        GUI.color = Color.red;
        GUI.HorizontalScrollbar(new Rect(200, 50, 200, 20), pos, value, 0, 100);
    }
}
