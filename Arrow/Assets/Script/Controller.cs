using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour, SceneController, Interaction
{
    public ArrowActionManager actionManager;
    public ArrowFactory factory;
    public GameObject bow;
    public GameObject target;
    public GameObject arrow;
    public Judger judger;
    public Vector3 direction;
    public UI ui;

    public int state = 0;
    private int arrowNumber = 0;
    private Queue<GameObject> hit_arrow = new Queue<GameObject>();
    public Vector3 wind = Vector3.zero; 
    private int[] direc = {1,-1,0};
    private void Start() {
        Director director = Director.getInstance();
        director.currentSceneController = this;
        factory = this.gameObject.AddComponent<ArrowFactory>();
        actionManager = this.gameObject.AddComponent<ArrowActionManager>() as ArrowActionManager;
        ui = this.gameObject.AddComponent<UI>();
        judger = this.gameObject.AddComponent<Judger>();
        // factory = Singleton<ArrowFactory>.Instance;
        loadResources();
        int x = Random.Range(0,3);
        int y = Random.Range(0,3);
        x = direc[x];
        y = direc[y];
        int level = Random.Range(1,5);
        wind = new Vector3(x, y, 0) * level;
    }

    public void loadResources() {
        bow = Instantiate(Resources.Load("Prefabs/bow", typeof(GameObject))) as GameObject;
        target = Instantiate(Resources.Load("Prefabs/target", typeof(GameObject))) as GameObject;
        arrow = factory.getArrow();
    }

    private void Update()
    {
    }

    public void moveArrowDirection(Vector3 to) {
        if (state <= 0) {
            return;
        }
        arrow.transform.rotation = Quaternion.LookRotation(to);
        bow.transform.rotation = Quaternion.LookRotation(to);
        direction = to;
    }

    public void reuse() {
        int tmp = hit_arrow.Count;
        for (int i = 0; i < tmp; i ++) {
            factory.freeArrow(hit_arrow.Dequeue());
        }
        arrowNumber = 0;
    }
    public void shoot(Vector3 force) {

        if (state > 0 && arrow != null) {
            // arrow = factory.getArrow();
            arrow.name = "arrow";
            actionManager.arrowFly(arrow, direction * 15, wind);
            arrowNumber ++;
        }
    }
    public void hit(GameObject arrow) {
        hit_arrow.Enqueue(arrow);
    }
    public void getArrow() {
        int x = Random.Range(0,3);
        int y = Random.Range(0,3);
        x = direc[x];
        y = direc[y];
        int level = Random.Range(1,5);
        wind = new Vector3(x, y, 0) * level;
        if (state == 1) {
            if (arrowNumber > 7) {
                setState(-1);
            }
        }
        arrow = factory.getArrow();
    }
    public void setState(int s) {
        state = s;
    }
    public void restart() {
        state = 0;
        arrowNumber = 0;
    }

    public int getState() {
        return state;
    }

    public string arrowState() {
        return arrow != null ? arrow.name : null;
    }

    public Vector3 getWind() {
        return wind;
    }
}
