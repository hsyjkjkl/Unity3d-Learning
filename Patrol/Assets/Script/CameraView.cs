using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    public GameObject follow;
    public float smothing = 5f;
    Vector3 offset;
    Quaternion rotate = Quaternion.Euler(30, 0, 0);

    void Start()
    {
        offset = new Vector3(0, 3, -3);
        
    }

    void FixedUpdate()
    {
        // Vector3 target = follow.transform.localPosition + offset;
        Vector3 target = follow.transform.TransformPoint(offset);
        Vector3 tmp = rotate.eulerAngles + follow.transform.rotation.eulerAngles;
        Quaternion r = Quaternion.Euler(tmp);
        transform.position = Vector3.Lerp(transform.position, target, smothing * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, r, smothing/2 * Time.deltaTime);
    }
}