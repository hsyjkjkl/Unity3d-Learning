using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour, SceneController, ISSActionCallback, Interaction {
    public FlyActionManager actionManager;
    public DiskFactory diskFactory;

    public Judger judger;
    public UserUI ui;
    public int trial = 10;
    public float time = 0;
    public int round = 1;
    public int n = 3;
    public int state = 0;

    public Queue<GameObject> diskQueue = new Queue<GameObject>();

    private void Awake()
    {
        Director director = Director.getInstance();
        director.currentSceneController = this;
        actionManager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        this.gameObject.AddComponent<DiskFactory>();
        this.gameObject.AddComponent<Judger>();
        diskFactory = Singleton<DiskFactory>.Instance;
        ui = gameObject.AddComponent<UserUI>() as UserUI;
        judger = Singleton<Judger>.Instance;
    }

    private void Update()
    {   
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

        if (time > 1.5)
        {   
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
    //游戏重新开始
    public void changeState(int a){
        state = a;
    }

    public void reset() {
        trial = 10;
        round = 1;
        time = 0;
    }
    
}