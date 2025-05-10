using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAttack : State
{

    public override string StateName => "Book_Attack";
    BookStateMachineContext context;
    private HealthComponent healthComponent;
    private Coroutine attackCoroutine;
    public BookAttack(BookStateMachineContext context)
    {
        this.context = context;
    }

    public override bool ShouldTransition()
    {
        if (!context.Agent.enabled)
        {
            return false;
        }
        if (context.HealthComponent.HealthPoints <= 0)
        {
            return false;
        }
        if (((context.Player.GetTransform().position - context.transform.position).sqrMagnitude - context.AttackDistance * context.AttackDistance) <= Mathf.Epsilon)
        {
            return true;
        }

        return false;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        context.Animator.enabled = false;
        healthComponent = context.Player.GetCustomComponent<HealthComponent>();
        healthComponent.OnDie += StopAttacking;
        attackCoroutine = context.StartCoroutine(StartAttacking());
    }
    public override void OnExit()
    {
        base.OnExit();
        healthComponent.OnDie -= StopAttacking;
        if (attackCoroutine != null)
        {
            context.StopCoroutine(attackCoroutine);
        }
        //context.FlameAttackParticleSystem.Stop();
    }

    public override void Update()
    {
        base.Update();
        var dir = context.Player.GetTransform().position - context.transform.position;
        dir.y = 0;
        dir.Normalize();
        context.transform.forward = dir;
        //context.FlameAttackParticleSystem.transform.forward = (context.Player.GetTransform().position - context.FlameAttackParticleSystem.transform.position).normalized;
    }

    private IEnumerator StartAttacking()
    {
        //context.FlameAttackParticleSystem.Play();
        healthComponent.Damage(context.DamagePerSecond);
        while (true)
        {
            yield return new WaitForSeconds(1);
            healthComponent.Damage(context.DamagePerSecond);
        }
    }
    private void StopAttacking()
    {
        if (attackCoroutine != null)
        {
            //context.FlameAttackParticleSystem.Stop();
            context.StopCoroutine(attackCoroutine);
        }
    }
}
