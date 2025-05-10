using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float _maxHealthPoints = 200;
    public float HealthPoints { get; protected set; }
    private bool _isReceivingDamage = false;
    public bool IsReceivingDamage => _isReceivingDamage;

    public float MaxHealth => _maxHealthPoints;
    public Action OnDamageReceived;
    public UnityAction OnDie;

    private void Awake()
    {
        HealthPoints = _maxHealthPoints;
    }

    /// <summary>
    /// Reduces the character's health, updates the recovery time
    /// and triggers some events
    /// </summary>
    /// <param name="points"></param>
    public virtual void Damage(float points)
    {
        if (HealthPoints <= 0)
            return;

        HealthPoints -= points;
        if (HealthPoints <= 0)
        {
            HealthPoints = 0;
            OnDie?.Invoke();
            return;
        }
        OnDamageReceived?.Invoke();
    }

    public void SetHealth(float health)
    {
        HealthPoints = health;
    }

    public void RestoreFullHealth()
    {
        SetHealth(MaxHealth);
    }

    public void SetReceivingDamageState(bool state) => _isReceivingDamage = state;

    /// <summary>
    /// Plays the dead animation and disables the controls
    /// </summary>
    public void Die()
    {
        Damage(HealthPoints);
    }
    
    public bool HasMaxHealth() => (HealthPoints / MaxHealth) == 1;
}