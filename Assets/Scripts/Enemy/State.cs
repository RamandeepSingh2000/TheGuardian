using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public abstract string StateName { get; }

    public virtual bool ShouldTransition()
    {
        return false;
    }
    public virtual void OnEnter()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void OnExit()
    {

    }
}
