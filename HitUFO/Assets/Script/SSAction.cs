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


