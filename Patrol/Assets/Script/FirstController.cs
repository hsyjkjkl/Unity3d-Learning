using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FirstController : MonoBehaviour, SceneController, Interaction{

    public int playerArea;
    public PatrolActionManager patrolActionManager;
    Judger judger;
    public UI ui;
    public PatrolFactory patrolFactory;
    public GameObject player;
    private List<GameObject> patrols; 
    private GameObject moveWall;
    private int state = 0;
    bool flag = true;
    private int ballCount = 0;
    void Awake() {
        Director director = Director.getInstance();
        director.currentSceneController = this;
        judger = gameObject.AddComponent<Judger>();
        patrolFactory = gameObject.AddComponent<PatrolFactory>();
        playerArea = 5;
        patrolActionManager = gameObject.AddComponent<PatrolActionManager>();
        gameObject.AddComponent<GameEventManager>();
        ui = gameObject.AddComponent<UI>() as UI;
        
    }
    private void Start()
    {
        loadResources();
        patrolFactory.StartPatrol();
        for (int i = 0; i < patrols.Count; i++) {
            patrolActionManager.Patrol(patrols[i]);
        }
        moveWall = GameObject.FindGameObjectWithTag("move");
    }
    public void loadResources() {
        Instantiate(Resources.Load<GameObject>("Prefabs/Plane")).name = "Plane";
        player = Instantiate(Resources.Load("Prefabs/Player"), new Vector3(13, 0, 13), Quaternion.identity) as GameObject;
        player.name = "Player";
        Instantiate(Resources.Load<GameObject>("Prefabs/Ball"), new Vector3(8, 0.5f, -17), Quaternion.identity).name = "Ball";
        Instantiate(Resources.Load<GameObject>("Prefabs/Ball"), new Vector3(-18, 0.5f, 12), Quaternion.identity).name = "Ball";
        Instantiate(Resources.Load<GameObject>("Prefabs/Ball"), new Vector3(-4, 0.5f, -17), Quaternion.identity).name = "Ball";
        patrols = patrolFactory.getPatrols();
        Camera.main.GetComponent<CameraView>().follow = player;
    }
    void Update() {
        if (state != 1) return;
        if (ballCount == 3 && flag) {
            moveWall.transform.localPosition += new Vector3(0,0,-2);
            flag = false;
        }
        for (int i = 0; i < patrols.Count; i++) {
            patrols[i].GetComponent<PatrolData>().playerArea = playerArea;
        }
    }

    void OnEnable() {
        GameEventManager.OnGoalLost += OnGoalLost;
        GameEventManager.OnFollowing += OnFollowing;
        GameEventManager.GameOver += GameOver;
        GameEventManager.Win += Win;
    }

    void OnDisable() {
        GameEventManager.OnGoalLost -= OnGoalLost;
        GameEventManager.OnFollowing -= OnFollowing;
        GameEventManager.GameOver -= GameOver;
        GameEventManager.Win -= Win;
    }

    public void OnGoalLost(GameObject patrol) {
        patrolActionManager.Patrol(patrol);
        judger.addScore();
    }

    public void OnFollowing(GameObject patrol) {
        patrolActionManager.Follow(player, patrol);
    }

    public void GameOver() {
        state = 0;
        StopAllCoroutines();
        patrolFactory.PausePatrol();
        player.GetComponent<Animator>().SetTrigger("death");
        patrolActionManager.DestroyAllActions();
    }

    public void Win() {
        state = 2;
        StopAllCoroutines();
        patrolFactory.PausePatrol();
        patrolActionManager.DestroyAllActions();
        
    }
    public int getState() {
        return state;
    }
    public void changeState(int a) {
        state = a;
    }

    public void PlayerPick() {
        player.GetComponent<Animator>().SetTrigger("attack");
    }
    public void movePlayer(Vector3 pos) {
        if (pos.x != 0 || pos.z != 0) {
            player.GetComponent<Animator>().SetBool("run", true);
        } else {
            player.GetComponent<Animator>().SetBool("run", false);
        }
        pos.x *= -2*Time.deltaTime;
        pos.z *= -2*Time.deltaTime;
        
        player.transform.Rotate(Vector3.up, -pos.x*50, Space.Self);
        // new Vector3(player.transform.localPosition.x + pos.x, player.transform.position.y, player.transform.localPosition.z + pos.z)
        // player.transform.LookAt();
        player.transform.Translate(-0.05f*pos.x, 0, -pos.z * 2);

    }

    public void addBall() {
        ballCount += 1;
    }
    public void reset() {
        SceneManager.LoadScene("Scenes/SampleScene");
    }
    public int GetScore() {
        return 1;
    }
}