using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour, SceneController, ISSActionCallback, Interaction {
    public IActionManager actionManager;
    public DiskFactory diskFactory;
    public ActionMode mode = ActionMode.NONE;

    public Judger judger;
    public UserUI ui;
    public int trial = 10;
    public float time = 0;
    public int round = 1;
    public int n = 3;
    public int state = 0;

    public Queue<GameObject> diskQueue = new Queue<GameObject>();
    public int used = 0;
    private void Awake()
    {
        Director director = Director.getInstance();
        director.currentSceneController = this;
        
        this.gameObject.AddComponent<DiskFactory>();
        this.gameObject.AddComponent<Judger>();
        diskFactory = Singleton<DiskFactory>.Instance;
        ui = gameObject.AddComponent<UserUI>() as UserUI;
        judger = Singleton<Judger>.Instance;
    }

    private void Update()
    {   
        if (mode == ActionMode.NONE) return;

        // if (state <= 0) {
            
        // }

        if (state <= 0 || state == 2) {
            return;
        }
        if (trial == 0 && round >= n) {
            time += Time.deltaTime;
            if (time > 3) {
                changeState(-2);
                time = 0;
            }
            return;
        }
        if (trial == 0 && state == 1)
        {
            state = 2;
            
        }
 
        if (trial == 0 && state == 3)
        {
                round = (round + 1);
                if (round > n) {
                    return;
                }
                trial = 10;
                state = 1;
        }

        if (time > 1.5 && used == 0)
        {   
            Debug.Log("Throw!");
            if (Singleton<Judger>.Instance.checkGame() == false) {
                changeState(-1);
                return;
            }
            ThrowDisk();
            time = 0;
        }
        else
        {
            time += Time.deltaTime;
        }
    }

    
    public void ThrowDisk() {
        int tmp = Random.Range(0, round);
        int num = 0;
        if (tmp < 0.9) {
            diskQueue.Enqueue(diskFactory.GetDisk(round));
            num = 1;
        }
        else if (tmp < 2) {
            diskQueue.Enqueue(diskFactory.GetDisk(round));
            diskQueue.Enqueue(diskFactory.GetDisk(round));
            num = 2;
        }
        else
        {
            diskQueue.Enqueue(diskFactory.GetDisk(round));
            diskQueue.Enqueue(diskFactory.GetDisk(round));
            diskQueue.Enqueue(diskFactory.GetDisk(round));
            num = 3;
        }
        used = num;
        for(int i = 0; i < num; i ++) {
            GameObject disk =  diskQueue.Dequeue();
            Vector3 position = new Vector3(0, 0, 0);
            float y = UnityEngine.Random.Range(-3f, 2f);
            position = new Vector3(-disk.GetComponent<DiskInfo>().target.x * 7, y, 0);
            disk.transform.position = position;
 
            disk.SetActive(true);
            
            actionManager.flyUFO(disk, disk.GetComponent<DiskInfo>().target,disk.GetComponent<DiskInfo>().speed);
        }
        trial --;
    }

    public void hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
 
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
 
            if (hit.collider.gameObject.GetComponent<DiskInfo>() != null && hit.collider.gameObject.GetComponent<DiskInfo>().hit != 1)
            {
                hit.collider.gameObject.GetComponent<DiskInfo>().hit = 1;
                judger.hit(hit.collider.gameObject);
                hit.collider.gameObject.transform.position = new Vector3(0, -20, 0);
                return;
            }
 
        }
    }
    public void loadResources() {

    }
    public void SSActionEvent(SSAction action) {

    }
    public int GetScore() {
        return Singleton<Judger>.Instance.getScore();
    }
    //游戏结束
    public int getState(){
        return state;
    }
    public void setMode(ActionMode m) {
        mode = m;
        if (mode == ActionMode.PHYSICS) {
            if (gameObject.GetComponent<PhysicsManager>() == null)
                actionManager = gameObject.AddComponent<PhysicsManager>() as PhysicsManager;
            else
                actionManager = gameObject.GetComponent<PhysicsManager>() as PhysicsManager;
        }
        else if (mode == ActionMode.MOVE) {
            if (gameObject.GetComponent<FlyActionManager>() == null)
                actionManager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
            else
                actionManager = gameObject.GetComponent<FlyActionManager>() as FlyActionManager;
        }
    }
    public void changeState(int a){
        state = a;
    }

    public void reset() {
        trial = 10;
        round = 1;
        time = 0;
    }
    
}