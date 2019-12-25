using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeFactory: MonoBehaviour{
    List<GameObject> activeList = new List<GameObject>();
    List<GameObject> freeList = new List<GameObject>();

    public GameObject getPipe() {
        GameObject pipe = null;
        GameObject img = GameObject.Find("ImageTarget");
        if (img != null) {
            if (freeList.Count > 0)
            {
                pipe = freeList[0].gameObject;
                freeList.Remove(freeList[0]);
                pipe.transform.Rotate(0,0,0);
            }
            else
            {
                pipe = Instantiate(Resources.Load<GameObject>("Prefabs/Pipe"), new Vector3(8, Random.Range(-3,3),0), Quaternion.identity);
            }
            pipe.SetActive(true);
            pipe.transform.parent = img.transform;
            float x = Random.Range(-1,1) > 0 ? 0.5333f : -0.5333f;
            pipe.transform.localPosition = new Vector3(x, Random.Range(-1f,1f),0);
        }

        
        return pipe;
    }

    public void free(GameObject pipe) {
        GameObject tmp = null;
        foreach (GameObject i in activeList)
        {
            if (pipe.GetInstanceID() == i.gameObject.GetInstanceID())
            {
                tmp = i;
                break;
            }
        }
        if (tmp != null) {
            tmp.gameObject.SetActive(false);
            freeList.Add(tmp);
            activeList.Remove(tmp);
        }
    }
}