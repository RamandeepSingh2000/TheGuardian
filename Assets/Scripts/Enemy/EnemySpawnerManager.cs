using System;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner[] _enemySpawners;

    public Action OnEnemyDied;
    public Action OnSpawnerDestroyed;

    public int DeadEnemies { get; private set; } = 0;
    public int TotalNumberOfPortals => _enemySpawners.Length;
    public int NumberOfActivePortals { get; private set; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        NumberOfActivePortals = TotalNumberOfPortals;
    }

    private void OnEnable()
    {
        for (int i = 0; i < _enemySpawners.Length; i++)
        {
            _enemySpawners[i].OnDestroyed += SpawnerDestroyed;
            _enemySpawners[i].OnEnemyDied += EnemyKilled;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _enemySpawners.Length; i++)
        {
            if (_enemySpawners[i] == null)
                continue;
            _enemySpawners[i].OnDestroyed -= SpawnerDestroyed;
            _enemySpawners[i].OnEnemyDied -= EnemyKilled;
        }
    }

    private void SpawnerDestroyed(EnemySpawner spawner)
    {
        spawner.OnEnemyDied -= EnemyKilled;
        spawner.OnDestroyed -= SpawnerDestroyed;
        NumberOfActivePortals--;
        OnSpawnerDestroyed?.Invoke();        
    }

    private void EnemyKilled()
    {
        DeadEnemies++;
        OnEnemyDied?.Invoke();
    }

}
