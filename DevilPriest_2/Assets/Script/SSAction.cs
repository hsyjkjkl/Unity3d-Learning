using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSAction : ScriptableObject {

    public bool enable = true;
    public bool destroy = false;

    public GameObject gameObject{get;set;}
    public Transform transform{get;set;}
    public ISSActionCallback callBack{get;set;}

    public virtual void Start()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}

public interface ISSActionCallback
{
    void SSActionEvent(SSAction action);
}

public class SSMoveToAction : SSAction
{
    public Vector3 target;
    public float speed;

    private SSMoveToAction() { }
    public static SSMoveToAction GetSSAction(Vector3 target, float speed)
    {
        SSMoveToAction action = ScriptableObject.CreateInstance<SSMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update()
    {
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        if (this.transform.position == target)
        {
            this.destroy = true;
            this.callBack.SSActionEvent(this);
        }
    }

    public override void Start()
    {
        
    }
}

public class CCSequenceAction : SSAction, ISSActionCallback
{
    public List<SSAction> sequence; 
    public int repeat = -1;
    public int start = 0;

    public static CCSequenceAction GetSSAcition(int repeat, int start, List<SSAction> sequence)
    {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.repeat = repeat;
        action.sequence = sequence;
        action.start = start;
        return action;
    }

    public override void Update()
    {
        if (sequence.Count == 0) return;
        if (start < sequence.Count)
        {
            sequence[start].Update();
        }
    }

    public void SSActionEvent(SSAction action)
    {
        action.destroy = false;
        this.start++;
        if (this.start >= sequence.Count)
        {
            this.start = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0)
            {
                this.destroy = true;
                this.callBack.SSActionEvent(this);
            }
        }
    }

    public override void Start()
    {
        foreach (SSAction action in sequence)
        {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callBack = this;
            action.Start();
        }
    }

    void OnDestroy()
    {
        foreach (SSAction action in sequence)
        {
            Destroy(action);
        }
    }
}

public class SSActionManager : MonoBehaviour
{
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingForAdd = new List<SSAction>();
    private List<int> waitingForDelete = new List<int>();

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

