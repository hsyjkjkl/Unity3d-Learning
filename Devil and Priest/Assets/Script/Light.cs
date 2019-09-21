using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light: MonoBehaviour {
    private void Start() {
        GameObject DirectionalLight = Instantiate (Resources.Load ("Prefabs/Directional Light", typeof(GameObject)), new Vector3(0, 3, 0), Quaternion.Euler(50, -30,0), null) as GameObject;
    }
}