using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFX : MonoBehaviour
{
    enum EnemyType
    {
        Mannequin,
        Lantern,
        Book
    }

    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private EnemyStateMachineContext _enemyStateMachine;

    private PoolableAudioSource _audioSource = null;

    private void OnEnable()
    {
        _enemyStateMachine.OnStartAttacking += OnStartAttacking;
        _enemyStateMachine.OnAttack += OnAttack;
        _enemyStateMachine.OnStopAttacking += OnStopAttacking;
        _enemyStateMachine.OnPossessed += OnPossessed;
        _enemyStateMachine.HealthComponent.OnDie += OnDie;
    }

    private void OnDisable()
    {
        _enemyStateMachine.OnStartAttacking -= OnStartAttacking;
        _enemyStateMachine.OnAttack -= OnAttack;
        _enemyStateMachine.OnStopAttacking -= OnStopAttacking;
        _enemyStateMachine.OnPossessed -= OnPossessed;
        _enemyStateMachine.HealthComponent.OnDie -= OnDie;
    }

    private void OnStartAttacking()
    {
        switch (_enemyType)
        {
            case EnemyType.Mannequin:
                break;
            case EnemyType.Lantern:
                _audioSource = SFXManager.Instance.Play3DSound(SFXCategory.Enemy, "LanternAttack", transform);
                break;
            case EnemyType.Book:
                break;
            default:
                break;
        }
    }

    private void OnAttack()
    {
        switch (_enemyType)
        {
            case EnemyType.Mannequin:
                SFXManager.Instance.Play3DSound(SFXCategory.Enemy, "MannequinAttack", transform.position);
                break;
            case EnemyType.Lantern:
                break;
            case EnemyType.Book:
                break;
            default:
                break;
        }
    }

    private void OnStopAttacking()
    {
        switch (_enemyType)
        {
            case EnemyType.Mannequin:
                break;
            case EnemyType.Lantern:
                if(_audioSource!=null)
                {
                    _audioSource.Deactivate();
                    _audioSource = null;
                }                    
                break;
            case EnemyType.Book:
                break;
            default:
                break;
        }
    }

    private void OnPossessed()
    {
        SFXManager.Instance.Play3DSound(SFXCategory.Enemy,"SoulPossession",transform.position);
    }

    private void OnDie()
    {
        SFXManager.Instance.Play3DSound(SFXCategory.Enemy, "Die", transform.position);
    }

}
