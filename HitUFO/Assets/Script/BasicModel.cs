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
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

	protected static T instance;

	public static T Instance {  
		get {  
			if (instance == null) { 
				instance = (T)FindObjectOfType (typeof(T));  
				if (instance == null) {  
					Debug.LogError ("An instance of " + typeof(T) +
					" is needed in the scene, but there is none.");  
				}  
			}  
			return instance;  
		}  
	}
}

public interface Interaction
{
	void hit(Vector3 pos);
    int GetScore();
    int getState();
	void changeState(int a);
	void reset();
}
