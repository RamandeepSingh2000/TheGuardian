using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private MovementController _movementController;

    [SerializeField] private PlayerHealthController _playerHealth;

    [SerializeField] private AttackController _attackController;

    private void OnEnable()
    {
        _movementController.OnJump += OnJump;
        _attackController.OnAttackChanged += OnAttackChanged;
        _playerHealth.OnHealthRestored += OnHealthRestored;

        for (int i = 0; i < _attackController.Attacks.Length; i++)
        {
            var attack = _attackController.Attacks[i];
            if (attack.AttackName == AttackName.Portal)
            {
                attack.OnStartShooting += () => OnShoot(attack.AttackName);                
                continue;
            }
            attack.OnShoot += () => OnShoot(attack.AttackName);
        }
    }

    private void OnDisable()
    {
        _attackController.OnAttackChanged -= OnAttackChanged;
        _playerHealth.OnHealthRestored -= OnHealthRestored;
        _movementController.OnJump -= OnJump;
    }

    private void OnHealthRestored()
    {
        SFXManager.Instance.Play2DSound(SFXCategory.Player, "HealthPotion");
    }

    private void OnAttackChanged(AttackName attackName)
    {
        SFXManager.Instance.Play2DSound(SFXCategory.Player, "AttackChanged");
    }

    private void OnShoot(AttackName attackName)
    {
        string attackSound = attackName.ToString();

        attackSound += "Attack";
        SFXManager.Instance.Play2DSound(SFXCategory.Player, attackSound);
    }    

    private void OnJump()
    {
        SFXManager.Instance.Play2DSound(SFXCategory.Player, "Jump");
    }
}
