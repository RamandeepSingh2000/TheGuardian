using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookDeath : State
{
    public override string StateName => "Book_Death";
    BookStateMachineContext context;
    public BookDeath(BookStateMachineContext context)
    {
        this.context = context;
    }

    public override bool ShouldTransition()
    {
        if (context.HealthComponent.HealthPoints <= 0)
        {
            return true;
        }
        return false;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        context.Animator.enabled = true;
        context.Animator.SetBool("Dead", true);
        //context.FlameParticleSystem.Stop();
        context.ColliderSetActive(false);
        context.IsDead = true;
        context.SpawnDeathEnergyOrb();
        context.SpawnItem();
    }
}
