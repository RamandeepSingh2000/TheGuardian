using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinChase : State
{
    public override string StateName => "Mannequin_Chase";
    private MannequinStateMachineContext context;
    private bool isReady;

    public MannequinChase(MannequinStateMachineContext context)
    {
        this.context = context;        
    }

    public override bool ShouldTransition()
    {
        if (context.HealthComponent.HealthPoints <= 0)
        {
            return false;
        }
        if ((context.Player.GetTransform().position - context.transform.position).sqrMagnitude > context.AttackDistance * context.AttackDistance)
        {
            return true;
        }

        return false;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        context.Animator.SetBool("IsAttacking", false);
        StartChasing();
    }

    public override void OnExit()
    {
        base.OnExit();
        isReady = false;
        context.Agent.enabled = false;
        context.StopAllCoroutines();
    }

    public override void Update()
    {
        base.Update();
        if (!isReady)
        {
            return;
        }
        context.Agent.destination = context.Player.GetTransform().position;
    }

    private void StartChasing()
    {
        context.Agent.enabled = true;
        context.Agent.stoppingDistance = context.AttackDistance;
        context.Agent.speed = context.Speed;
        isReady = true;
    }
}
