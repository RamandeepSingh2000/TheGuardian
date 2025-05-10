using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinDeath : State
{
    public override string StateName => "Mannequin_Death";
    private MannequinStateMachineContext context;
    public MannequinDeath(MannequinStateMachineContext context)
    {
        this.context = context;
    }

    public override bool ShouldTransition()
    {
        if(context.HealthComponent.HealthPoints <= 0)
        {
            return true;
        }
        return false;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        context.Animator.SetTrigger("Death");
        context.ColliderSetActive(false);
        context.IsDead = true;
        context.SpawnDeathEnergyOrb();
        context.SpawnItem();
    }
}
