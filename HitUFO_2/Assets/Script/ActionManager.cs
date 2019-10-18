using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionMode { PHYSICS, MOVE, NONE}
public class ActionManager : MonoBehaviour{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingForAdd = new List<SSAction>();
    private List<int> waitingForDelete = new List<int>();

    private Controller controller;
    private void Start()
    {
        
    }
    protected void FixedUpdate()
    {   
        foreach (SSAction action in waitingForAdd)
        {
            actions[action.GetInstanceID()] = action;
        }
        
        waitingForAdd.Clear();

        foreach (KeyValuePair<int,SSAction> pair in actions)
        {
            SSAction action = pair.Value;
            if (action.destroy)
            {
                waitingForDelete.Add(action.GetInstanceID());
            } else if (action.enable)
            {
                action.FixedUpdate();
            }
        }
        

        foreach (int key in waitingForDelete)
        {
            SSAction action = actions[key];
            actions.Remove(key);
            Destroy(action);
        }
        
        waitingForDelete.Clear();
    }

    protected void Update()
    {
        foreach (SSAction action in waitingForAdd)
        {
            actions[action.GetInstanceID()] = action;
        }
        
        waitingForAdd.Clear();

        foreach (KeyValuePair<int,SSAction> pair in actions)
        {
            SSAction action = pair.Value;
            if (action.destroy)
            {
                waitingForDelete.Add(action.GetInstanceID());
            } else if (action.enable)
            {
                action.Update();
            }
        }
        

        foreach (int key in waitingForDelete)
        {
            SSAction action = actions[key];
            actions.Remove(key);
            Destroy(action);
        }
        
        waitingForDelete.Clear();
    }

    public void RunAction(GameObject gameObject, SSAction action, ISSActionCallback callback)
    {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callBack = callback;
        
        waitingForAdd.Add(action);
        action.Start();
    }

}