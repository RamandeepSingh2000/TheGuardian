using UnityEngine;

namespace Player.UI
{
    public class HUDPresenter : MonoBehaviour
    {
        [SerializeField] private HUDView _view;

        private PlayerHealthController _playerHealthController;
        private AttackController _attackController;
        private EnemySpawnerManager _enemySpawnerManager;

        private HealthComponent _currentPortalHealth = null;

        void Start()
        {
            var playerController = GameManager.Instance.Player;
            _playerHealthController = playerController.GetCustomComponent<PlayerHealthController>();

            if (_playerHealthController != null)
                _playerHealthController.OnHealthChanged += (points) => UpdateHealth(points);

            _attackController = playerController.GetCustomComponent<AttackController>();

            if (_attackController != null)
            {
                _attackController.OnAttackChanged += UpdateCurrentWeapon;
                foreach (var attack in _attackController.Attacks)
                {
                    attack.OnStatusChanged += (status) => UpdateAttackStatus(attack.AttackName, status);
                    attack.OnAmmunitionChanged += (ammo) => UpdateAttackAmmunition(attack.AttackName, ammo / attack.MaxAmmunition);
                    UpdateAttackAmmunition(attack.AttackName, attack.Ammunition);
                }

                var portalAttack = _attackController.GetAttack(AttackName.Portal) as PortalAttack;
                if (portalAttack != null)
                {
                    portalAttack.OnAmmunitionChanged +=
                    (ammo) => _view.UpdateDeathEnergyPercentage(ammo / portalAttack.MaxAmmunition);
                    portalAttack.OnCurrentPortalChanged += UpdatePortalHealthReference;
                }
            }

            _enemySpawnerManager = FindObjectOfType<EnemySpawnerManager>();

            if (_enemySpawnerManager != null)
            {
                _enemySpawnerManager.OnEnemyDied += IncreaseEnemyKillCount;
                _enemySpawnerManager.OnSpawnerDestroyed += UpdatePortalsStats;

                UpdatePortalsStats();
            }
        }

        private void UpdateHealth(float healthPoints)
        {
            float percentage = (healthPoints / _playerHealthController.MaxHealth);
            _view.UpdateHealth(percentage);
        }

        private void UpdatePortalHealthReference(HealthComponent healthComponent)
        {
            if (_currentPortalHealth != null)
            {
                _currentPortalHealth.OnDamageReceived -= UpdatePortalHealth;
                _currentPortalHealth.OnDie -= UpdatePortalHealth;
            }

            _currentPortalHealth = healthComponent;
            if (_currentPortalHealth != null)
            {
                _currentPortalHealth.OnDamageReceived += UpdatePortalHealth;
                _currentPortalHealth.OnDie += UpdatePortalHealth;
            }
            UpdatePortalHealth();
        }

        private void UpdatePortalHealth()
        {
            if (_currentPortalHealth != null)
            {
                _view.SetPortalLifePercentage(_currentPortalHealth.HealthPoints / _currentPortalHealth.MaxHealth);
                if (_currentPortalHealth.HealthPoints <= 0)
                    UpdatePortalHealthReference(null);
            }                
            else _view.SetPortalLifePercentage(1);
        }

        private void UpdateCurrentWeapon(AttackName attack) => _view.SelectPower(attack);

        private void IncreaseEnemyKillCount()
        {
            _view.SetNumberOfEnemiesKilled(_enemySpawnerManager.DeadEnemies);
        }

        private void UpdatePortalsStats()
        {
            _view.SetPortalsStats(_enemySpawnerManager.NumberOfActivePortals, _enemySpawnerManager.TotalNumberOfPortals);
        }

        private void UpdateAttackStatus(AttackName attack, BaseAttack.Status status)
        {
            if (status == BaseAttack.Status.Ready)
                _view.EnablePower(attack);
            else _view.DisablePower(attack);
        }

        private void UpdateAttackAmmunition(AttackName attackName, float ammunition)
        {
            _view.SetPowerAmmunition(attackName, ammunition);
        }
    }
}
