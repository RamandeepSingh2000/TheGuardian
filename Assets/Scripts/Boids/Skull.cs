using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    [SerializeField] HealthComponent healthComponent;
    [SerializeField] Boid boid;
    [SerializeField] EnemyDeathEnergyOrb deathEnergyOrbPrefab;
    [SerializeField] float deathEnergyAmount;
    [SerializeField] IPlayer player;
    private void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();   
    }
    private void OnEnable()
    {
        healthComponent.OnDie += OnDie;
    }
    private void OnDisable()
    {
        healthComponent.OnDie -= OnDie;
    }
    
    private void OnDie()
    {
        var orb = Instantiate(deathEnergyOrbPrefab, transform.position, Quaternion.identity);
        orb.Init(player, deathEnergyAmount);
        boid.RemoveBoid();
        Destroy(gameObject);
    }
}
