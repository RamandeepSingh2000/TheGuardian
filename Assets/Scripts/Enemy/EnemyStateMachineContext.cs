using Player;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyStateMachineContext : StateMachineContext
{
    IPlayer player;
    [SerializeField]
    private float attackDistance = 1.0f;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int damagePerSecond = 5;
    [SerializeField]
    private HealthComponent healthComponent;
    [SerializeField]
    private Collider enemyCollider;
    [SerializeField]
    private Animator animator;
    [Header("Death Energy")]
    [SerializeField] EnemyDeathEnergyOrb enemyDeathEnergyOrbPrefab;
    [SerializeField] float deathEnergyAmount = 20f;
    [Header("Item Spawn")]
    [SerializeField] GameObject itemToSpawnOnDeath;
    [SerializeField][Range(0, 1f)] float spawnProbability;
    [SerializeField] LayerMask groundLayer;

    public NavMeshAgent Agent => agent;
    public float AttackDistance => attackDistance;
    public float Speed => speed;
    public int DamagePerSecond => damagePerSecond;
    public HealthComponent HealthComponent => healthComponent;
    public Animator Animator => animator;
    public IPlayer Player => player;
    protected bool isfrozen;
    public bool IsFrozen => isfrozen;

    public Action OnStartAttacking;
    public Action OnAttack;
    public Action OnStopAttacking;
    public Action OnPossessed;

    public abstract Transform SoulTargetTransform { get; }
    private bool isDead;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
        set
        {
            isDead = value;
            stateMachine.IsRunning = !isDead;
        }
    }
    [HideInInspector] public bool isMarked;

    protected override void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        player = FindObjectOfType<PlayerController>();
        base.Awake();
        IsDead = true;
        healthComponent.OnDie += UnFreeze;
    }
    private void OnDestroy()
    {
        healthComponent.OnDie -= UnFreeze;
    }

    public void ColliderSetActive(bool isActive)
    {
        enemyCollider.enabled = isActive;
    }
    public void Restore()
    {
        animator.enabled = true;
        HealthComponent.RestoreFullHealth();
        StartCoroutine(WakeUpAndRevive());
        OnPossessed?.Invoke();
    }
    public void SpawnDeathEnergyOrb()
    {
        var orb = Instantiate(enemyDeathEnergyOrbPrefab, transform.position, Quaternion.identity);
        orb.Init(player, deathEnergyAmount);
    }
    public void SpawnItem()
    {
        if (itemToSpawnOnDeath != null && UnityEngine.Random.Range(0, 1f) < spawnProbability)
        {
           
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hitInfo, 5f, groundLayer))
            {
                Instantiate(itemToSpawnOnDeath, hitInfo.point, Quaternion.identity);
            }            
        }
    }
    public virtual void Freeze()
    {
        if (isDead)
        {
            return;
        }
        stateMachine.IsRunning = false;
        animator.speed = 0;
        agent.enabled = false;
        isfrozen = true;
    }
    public virtual void UnFreeze()
    {
        if (!IsFrozen)
        {
            return;
        }
        animator.speed = 1;
        agent.enabled = true;
        isfrozen = false;
        stateMachine.IsRunning = true;
    }
    protected abstract IEnumerator WakeUp();
    private IEnumerator WakeUpAndRevive()
    {
        yield return WakeUp();
        isMarked = false;
        IsDead = false;
    }
}
