using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Controller : MonoBehaviour, SceneController, Interaction{

    public PipeFactory pipeFactory;
    public int score = 0;
    float time = 0;
    public int state = 1;
    public void loadResources() {

    }
    private void Start()
    {
        score = 0;
        pipeFactory =  this.gameObject.AddComponent<PipeFactory>();
        Director director = Director.getInstance();
        director.currentSceneController = this;
    }
    private void Update() {
        if (state == 1) {
            GameObject tmp = null;
            if (time > 3)
            {   
                tmp = pipeFactory.getPipe();
                time = 0;
            }
            else
            {
                time += Time.deltaTime;
            }
        }
            
    }

    public int getState() {
        return state;
    }

    public void addScore() {
        score += 1;
    }

    public int getScore() {
        return score;
    }
}