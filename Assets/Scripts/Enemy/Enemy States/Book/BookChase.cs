using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookChase : State
{
    public override string StateName => "Book_Chase";
    BookStateMachineContext context;
    private bool isReady;
    public BookChase(BookStateMachineContext context)
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
        //context.OriginFixBookAnimator.enabled = true;
        if (context.Animator.GetCurrentAnimatorStateInfo(0).IsName("BookFall"))
        {
            context.StartCoroutine(WaitForStanding());
        }
        else
        {
            StartChasing();
        }
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

    IEnumerator WaitForStanding()
    {
        context.Animator.SetBool("Dead", false);
        while (!context.Animator.GetCurrentAnimatorStateInfo(0).IsName("LanternStand"))
        {
            yield return null;
        }
        while (context.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
        {
            yield return null;
        }

        context.ColliderSetActive(true);
       // context.FlameParticleSystem.Play();
        StartChasing();
    }

    private void StartChasing()
    {
        context.Agent.enabled = true;
        context.Agent.stoppingDistance = context.AttackDistance;
        context.Agent.speed = context.Speed;
        isReady = true;
    }
}
