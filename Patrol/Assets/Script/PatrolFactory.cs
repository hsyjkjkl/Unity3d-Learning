using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PatrolFactory : MonoBehaviour
{
    private GameObject patrol = null;
    private List<GameObject> used = new List<GameObject>();
    private Vector3[] vec = {new Vector3(13,0,13), new Vector3(0,0,13), new Vector3(-13,0,13), 
                            new Vector3(13,0,0), new Vector3(0,0,0), new Vector3(-13,0,0), 
                            new Vector3(13,0,-13), new Vector3(0,0,-13), new Vector3(-13,0,-13)};

    public List<GameObject> getPatrols() {

        for (int i = 1; i < 8; i ++) {
            patrol = Instantiate(Resources.Load<GameObject>("Prefabs/Zombie"));
            patrol.name = "Zombie";
            patrol.AddComponent<PatrolData>();
            patrol.transform.position = vec[i];
            patrol.GetComponent<PatrolData>().patrolArea = i + 1;
            patrol.GetComponent<PatrolData>().playerArea = 1;
            patrol.GetComponent<PatrolData>().seePlayer = false;
            patrol.GetComponent<PatrolData>().tracking = false;
            patrol.GetComponent<PatrolData>().onCollison = false;
            patrol.GetComponent<Animator>().SetBool("pause", true);
            used.Add(patrol);
        }
        patrol = Instantiate(Resources.Load<GameObject>("Prefabs/Zombie"));
        patrol.name = "Zombie";
        patrol.AddComponent<PatrolData>();
        patrol.transform.position = new Vector3(3,0,3);
        patrol.GetComponent<PatrolData>().patrolArea = 5;
        patrol.GetComponent<PatrolData>().playerArea = 1;
        patrol.GetComponent<PatrolData>().seePlayer = false;
        patrol.GetComponent<PatrolData>().tracking = false;
        patrol.GetComponent<PatrolData>().onCollison = false;
        patrol.GetComponent<Animator>().SetBool("pause", true);
        used.Add(patrol);
        return used;
    }
    public void StartPatrol() {
        for (int i = 0; i < used.Count; i++) {
            used[i].gameObject.GetComponent<Animator>().SetBool("pause", false);
            used[i].gameObject.GetComponent<Animator>().SetBool("track", false);
        }
    }
    public void PausePatrol() {
        for (int i = 0; i < used.Count; i++) {
            used[i].gameObject.GetComponent<Animator>().SetBool("pause", true);
        }
    }
}

public class PatrolData : MonoBehaviour {
    public bool seePlayer;
    public bool tracking;
    public bool onCollison;
    public int patrolArea;
    public int playerArea;
    public GameObject player;
    public bool withTeammate = false;
}
