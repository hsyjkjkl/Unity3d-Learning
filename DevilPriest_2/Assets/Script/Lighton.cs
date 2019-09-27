using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighton: MonoBehaviour {
    private void Start() {
        GameObject DirectionalLight = Instantiate (Resources.Load ("Prefabs/Directional Light", typeof(GameObject)), new Vector3(0, 10, 0), Quaternion.Euler(50, -30,0), null) as GameObject;
    }
}