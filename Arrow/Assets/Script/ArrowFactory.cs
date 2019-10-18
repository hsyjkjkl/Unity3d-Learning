using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFactory : MonoBehaviour {
    public GameObject arrow = null;
    private List<GameObject> activeList = new List<GameObject>();
    private List<GameObject> freeList = new List<GameObject>();
    public GameObject getArrow() {
        if (freeList.Count > 0) {
            arrow = freeList[0].gameObject;
            freeList.Remove(freeList[0]);
            arrow.GetComponent<Rigidbody>().isKinematic = true;
            
        }
        else {
            arrow = Instantiate(Resources.Load("Prefabs/arrow", typeof(GameObject))) as GameObject;
        }

        arrow.transform.rotation = Quaternion.Euler(0,0,0);
        arrow.transform.position = new Vector3(-0.1f, 0.85f, -9.7f);
        arrow.SetActive(true);
        arrow.name = "ready";
        activeList.Add(arrow);
        return arrow;
    }

    public void freeArrow(GameObject a) {
        for (int i  = 0; i < activeList.Count; i ++) {
            if (a.GetInstanceID() == activeList[i].gameObject.GetInstanceID()) {
                activeList[i].gameObject.SetActive(false);
                freeList.Add(activeList[i]);
                activeList.Remove(activeList[i]);
                break;
            }
        }
    }
}