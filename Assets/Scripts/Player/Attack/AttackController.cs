using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public enum AttackName
    {
        Basic,
        Multiple,
        Freeze,
        Portal
    }
    public class AttackController : MonoBehaviour, IInjectableInputActions
    {
        [SerializeField] private BaseAttack[] _attacks;
        [SerializeField] private MeshRenderer _wandGem;

        public BaseAttack[] Attacks => _attacks;
        private BaseAttack _currentAttack;

        private PlayerInputActions _inputActions;
        public Action<AttackName> OnAttackChanged;

        public BaseAttack CurrentAttack => _currentAttack;

        //Delegates
        private Action<InputAction.CallbackContext> _shootPerformedAction;
        private Action<InputAction.CallbackContext> _shootCanceledAction;
        private Action<InputAction.CallbackContext> _defaultAttackPerformedAction;
        private Action<InputAction.CallbackContext> _specialAttack1PerformedAction;
        private Action<InputAction.CallbackContext> _specialAttack2PerformedAction;
        private Action<InputAction.CallbackContext> _portalAttackPerformedAction;

        private void Awake()
        {
            ChangeAttack(AttackName.Basic);
        }

        public void InjectInputActions(PlayerInputActions inputActions)
        {
            _inputActions = inputActions;
            EnableInputs();
        }

        public void EnableInputs()
        {
            _shootPerformedAction = (_) => StartAttacking();
            _shootCanceledAction = (_) => StopAttacking();
            _defaultAttackPerformedAction = (_) => ChangeAttack(AttackName.Basic);
            _specialAttack1PerformedAction = (_) => ChangeAttack(AttackName.Multiple);
            _specialAttack2PerformedAction = (_) => ChangeAttack(AttackName.Freeze);
            _portalAttackPerformedAction = (_) => ChangeAttack(AttackName.Portal);

            _inputActions.Attack.Enable();
            _inputActions.Attack.Shoot.performed += _shootPerformedAction;
            _inputActions.Attack.Shoot.canceled += _shootCanceledAction;
            _inputActions.Attack.DefaultWeapon.performed += _defaultAttackPerformedAction;
            _inputActions.Attack.SpecialAttack1.performed += _specialAttack1PerformedAction;
            _inputActions.Attack.SpecialAttack2.performed += _specialAttack2PerformedAction;
            _inputActions.Attack.PortalAttack.performed += _portalAttackPerformedAction;
        }

        public void DisableInputs()
        {
            _inputActions.Attack.Disable();
            _inputActions.Attack.Shoot.performed -= _shootPerformedAction;
            _inputActions.Attack.Shoot.canceled -= _shootCanceledAction;
            _inputActions.Attack.DefaultWeapon.performed -= _defaultAttackPerformedAction;
            _inputActions.Attack.SpecialAttack1.performed -= _specialAttack1PerformedAction;
            _inputActions.Attack.SpecialAttack2.performed -= _specialAttack2PerformedAction;
            _inputActions.Attack.PortalAttack.performed -= _portalAttackPerformedAction;
        }

        public void GoBackToDefaultAttack()
        {
            ChangeAttack(AttackName.Basic);
        }

        public void ChangeAttack(AttackName attackName)
        {
            var attack = GetAttack(attackName);
            if (attack.CurrentStatus == BaseAttack.Status.Unavailable)
                return;
            _currentAttack?.StopShooting();
            _currentAttack = attack;
            _wandGem.materials[1].color = _currentAttack.GemColor;
            OnAttackChanged?.Invoke(attackName);
        }

        public void StartAttacking() => _currentAttack?.StartShooting();

        public void StopAttacking() => _currentAttack?.StopShooting();

        public void ShootCurrentAttack() => _currentAttack?.Shoot();

        public void ReloadAttack(AttackName attackName, float ammo) => GetAttack(attackName).IncreaseAmmo(ammo);

        public BaseAttack GetAttack(AttackName attackName) => _attacks.FirstOrDefault(a => a.AttackName == (attackName));

        private void Update()
        {
            if (_currentAttack.AttackName != AttackName.Basic && !_currentAttack.CanShoot())
            {
                _currentAttack.StopShooting();
                GoBackToDefaultAttack();
            }
        }
    }

}
