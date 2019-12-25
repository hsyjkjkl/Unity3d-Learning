using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Director : System.Object
{
    // 单例模式，只能有一个实例，通过getInstance访问
    private static Director _instance;

    // 管理着SceneController，通过它来间接管理场景中的对象
    public SceneController currentSceneController { get; set; }

    public static Director getInstance() {
        if (_instance == null) {
            _instance = new Director ();
        }
        return _instance;
    }
}

public interface SceneController {
    void loadResources ();
    int getState();

    void addScore();
    int getScore();
}
public interface Interaction
{
}