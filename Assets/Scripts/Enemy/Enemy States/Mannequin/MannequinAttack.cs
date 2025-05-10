using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinAttack : State
{
    public override string StateName => "Mannequin_Attack";
    private MannequinStateMachineContext context;
    private HealthComponent healthComponent;
    private Coroutine attackCoroutine;
    public MannequinAttack(MannequinStateMachineContext context)
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
        context.Animator.SetBool("IsAttacking", true);
        healthComponent = context.Player.GetCustomComponent<HealthComponent>();
        healthComponent.OnDie += StopAttacking;
        attackCoroutine = context.StartCoroutine(StartAttacking());
    }
    public override void OnExit()
    {
        base.OnExit();
        healthComponent.OnDie -= StopAttacking;
        if(attackCoroutine != null)
        {
            context.StopCoroutine(attackCoroutine);
        }        
    }

    public override void Update()
    {
        base.Update();
        context.transform.forward = (context.Player.GetTransform().position - context.transform.position).normalized;
    }

    private IEnumerator StartAttacking()
    {
        while (!context.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            yield return null;
        }
        context.OnStartAttacking?.Invoke();

        var animationTime = context.Animator.GetCurrentAnimatorStateInfo(0).length;
        var animationFinishTime = animationTime - context.AttackTime;
        if(animationFinishTime < 0)
        {
            animationFinishTime = 0;
        }
        while (true)
        {
            yield return new WaitForSeconds(context.AttackTime);
            if (!context.IsFrozen)
            {
                healthComponent.Damage(context.AttackDamage);
                context.OnAttack?.Invoke();
                //SFXManager.Instance.Play2DSound(SFXCategory.Enemy, "MannequinAttack");
            }
            yield return new WaitForSeconds(animationFinishTime);
        }
    }

    private void StopAttacking()
    {
        if (attackCoroutine != null)
        {
            context.StopCoroutine(attackCoroutine);
            context.OnStopAttacking?.Invoke();
        }
    }
}
