using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MannequinStateMachineContext : EnemyStateMachineContext
{
    public override Transform SoulTargetTransform => transform;
    [SerializeField] float attackDamage;
    [SerializeField] float attackTime;
    public float AttackDamage => attackDamage;
    public float AttackTime => attackTime;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void AddStates()
    {
        stateMachine.AddState(new MannequinChase(this));
        stateMachine.AddState(new MannequinAttack(this));
        stateMachine.AddState(new MannequinDeath(this));
    }

    protected override IEnumerator WakeUp()
    {
        if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Animator.SetTrigger("Stand");
            while (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Stand"))
            {
                yield return null;
            }
            while (Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.95f)
            {
                yield return null;
            }
        }
        ColliderSetActive(true);
    }
}
