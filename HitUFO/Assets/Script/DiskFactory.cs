using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{
    public GameObject disk = null;
    private List<DiskInfo> activeList = new List<DiskInfo>();
    private List<DiskInfo> freeList = new List<DiskInfo>();
    int rand1 = 6, rand2 = 9, rand3 = 13;
    public int difficulty = 0;
    public GameObject GetDisk(int round)
    {
        GameObject newDisk = null;
        if (freeList.Count > 0)
        {
            newDisk = freeList[0].gameObject;
            freeList.Remove(freeList[0]);
        }
        else
        {
            newDisk = Instantiate(Resources.Load<GameObject>("Prefabs/Disk"), Vector3.zero, Quaternion.identity);
            
            newDisk.AddComponent<DiskInfo>();
        }

        switch(round) {
            case 1: {
                difficulty = Random.Range(0, rand1);
                break;
            }
            case 2: {
                difficulty = Random.Range(0, rand2);
                break;
            }
            case 3: {
                difficulty = Random.Range(0, rand3);
                break;
            }
        }

        if (difficulty < rand1 - 1) {
            newDisk.GetComponent<DiskInfo>().color = Color.white;
            newDisk.GetComponent<DiskInfo>().speed = 5.0f;
            float RanX = Random.Range(-1f, 1f) < 0 ? -2 : 2;
            newDisk.GetComponent<DiskInfo>().target = new Vector3(RanX, 1, 0);
            newDisk.GetComponent<Renderer>().material.color = Color.white;
        }
        else if (difficulty < rand2 - 1) {
            newDisk.GetComponent<DiskInfo>().color = Color.red;
            newDisk.GetComponent<DiskInfo>().speed = 7.0f;
            float RanX = Random.Range(-1f, 1f) < 0 ? -2 : 2;
            newDisk.GetComponent<DiskInfo>().target = new Vector3(RanX, 1, 0);
            newDisk.GetComponent<Renderer>().material.color = Color.red;
        }
        else {
            newDisk.GetComponent<DiskInfo>().color = Color.black;
            newDisk.GetComponent<DiskInfo>().speed = 9.0f;
            float RanX = Random.Range(-1f, 1f) < 0 ? -2 : 2;
            newDisk.GetComponent<DiskInfo>().target = new Vector3(RanX, 1, 0);
            newDisk.GetComponent<Renderer>().material.color = Color.black;
        }

        activeList.Add(newDisk.GetComponent<DiskInfo>());
        return newDisk;
    }
    public void freeDisk(GameObject disk) {
        DiskInfo tmp = null;
        foreach (DiskInfo i in activeList)
        {
            if (disk.GetInstanceID() == i.gameObject.GetInstanceID())
            {
                tmp = i;
                break;
            }
        }
        if (tmp != null) {
            tmp.gameObject.SetActive(false);
            tmp.hit = 0;
            freeList.Add(tmp);
            activeList.Remove(tmp);
        }
    }
}
public class DiskInfo : MonoBehaviour {
    public Vector3 pos;
    public Color color;
    public float speed;
    public Vector3 target;
    public int hit = 0;
}
