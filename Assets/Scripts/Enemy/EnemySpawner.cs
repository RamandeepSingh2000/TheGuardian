using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private EnemySoul enemySoulPrefab;
    [SerializeField]
    private List<EnemyStateMachineContext> enemyContext;
    [SerializeField]
    private float spawnRate = 1f;
    [SerializeField]
    Transform gfx;
    [SerializeField] HealthComponent healthComponent;

    public Action OnSoulSpawned;
    public Action OnEnemyDied;
    public Action<EnemySpawner> OnDestroyed;
    private float initScale;
    private void Awake()
    {
        initScale = gfx.localScale.x;
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(Spawn), 1, 1f / spawnRate);
    }

    private void OnEnable()
    {
        healthComponent.OnDamageReceived += OnDamaged;
        healthComponent.OnDie += OnDie;

        for (int i = 0; i < enemyContext.Count; i++)
        {
            enemyContext[i].HealthComponent.OnDie += EnemyDied;
        }
    }
    private void OnDisable()
    {
        healthComponent.OnDamageReceived -= OnDamaged;
        healthComponent.OnDie -= OnDie;

        for (int i = 0; i < enemyContext.Count; i++)
            enemyContext[i].HealthComponent.OnDie -= EnemyDied;
    }

    public void EnemyDied()
    {
        OnEnemyDied?.Invoke();
    }

    private void Spawn()
    {
        foreach (var enemy in enemyContext)
        {
            if (enemy.IsDead && !enemy.isMarked)
            {
                enemy.isMarked = true;
                var soul = Instantiate(enemySoulPrefab, transform.position, Quaternion.identity);
                soul.SetTarget(enemy);
                OnSoulSpawned?.Invoke();
                SFXManager.Instance.Play3DSound(SFXCategory.Enemy, "SoulSpawned", transform.position);
                return;
            }
        }
    }
    private void OnDamaged()
    {
        var newScale = gfx.localScale;
        newScale.x = initScale - Mathf.Lerp(0, initScale, 1 - healthComponent.HealthPoints / healthComponent.MaxHealth);
        gfx.localScale = newScale;
    }
    private void OnDie()
    {
        OnDestroyed?.Invoke(this);        
        Destroy(gameObject, 0.1f);
        SFXManager.Instance.Play3DSound(SFXCategory.Enemy, "PortalClosed", transform.position);
    }
}
