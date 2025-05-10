using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class LanternStateMachineContext : EnemyStateMachineContext
{
    [SerializeField] ParticleSystem flameParticleSystem;
    [SerializeField] ParticleSystem flameAttackParticleSystem;

    public ParticleSystem FlameParticleSystem => flameParticleSystem;
    public ParticleSystem FlameAttackParticleSystem => flameAttackParticleSystem;

    public override Transform SoulTargetTransform => flameParticleSystem.transform;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void AddStates()
    {
        stateMachine.AddState(new LanternChase(this));
        stateMachine.AddState(new LanternAttack(this));
        stateMachine.AddState(new LanternDeath(this));
    }
    public override void Freeze()
    {
        base.Freeze();
        flameAttackParticleSystem.Stop();
    }
    public override void UnFreeze()
    {
        base.UnFreeze();
        if(CurrentState == "Lantern_Attack")
        {
            flameAttackParticleSystem.Play();
        }
    }
    protected override IEnumerator WakeUp()
    {
        Animator.enabled = true;
        if (Animator.GetBool("Dead"))
        {            
            Animator.SetBool("Dead", false);
            while (!Animator.GetCurrentAnimatorStateInfo(0).IsName("LanternStand"))
            {
                yield return null;
            }
            while (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
            {
                yield return null;
            }
        }       

        ColliderSetActive(true);
        FlameParticleSystem.Play();
    }
}
