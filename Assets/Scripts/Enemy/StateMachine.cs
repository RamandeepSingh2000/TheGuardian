using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private readonly List<State> states = new();
    private State currentState = null;
    public string CurrentState => currentState.StateName;
    public bool IsRunning { get; set; }
    public void AddState(State state)
    {
        states.Add(state);
    }

    public void Update()
    {
        if (!IsRunning)
        {
            return;
        }
        foreach (var state in states)
        {
            if(currentState == state)
            {
                continue;
            }
            if (state.ShouldTransition())
            {
                currentState?.OnExit();                
                currentState = state;
                currentState.OnEnter();
                break;
            }
        }

        currentState?.Update();
    }
}
