using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookStateMachineContext : EnemyStateMachineContext
{
    public override Transform SoulTargetTransform => transform;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void AddStates()
    {
        stateMachine.AddState(new BookChase(this));
        stateMachine.AddState(new BookAttack(this));
        stateMachine.AddState(new BookDeath(this));
    }

    protected override IEnumerator WakeUp()
    {
        throw new System.NotImplementedException();
    }
}
