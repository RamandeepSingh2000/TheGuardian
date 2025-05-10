using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateMachineContext : MonoBehaviour
{
    protected StateMachine stateMachine;
    public string CurrentState => stateMachine.CurrentState;
    protected virtual void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        AddStates();
    }
    protected abstract void AddStates();
}
