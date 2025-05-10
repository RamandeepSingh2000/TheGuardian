using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour, IPlayer, IPausable
    {
        [SerializeField] private MovementController _movementController;
        [SerializeField] private AttackController _attackController;
        [SerializeField] private PlayerHealthController _healthController;
        [SerializeField] private Transform _deathEnergySphere;
        private PlayerInputActions _inputActions;

        private bool _triggerOnGamePause = true;
        public bool TriggerOnGamePaused { get => _triggerOnGamePause; set => _triggerOnGamePause = value; }

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Enable();
            _movementController.InjectInputActions(_inputActions);
            _attackController.InjectInputActions(_inputActions);
        }

        private void OnEnable()
        {
            EnableInputs();
            GameManager.Instance.OnGamePaused += OnGamePaused;
            GameManager.Instance.OnGameResumed += OnGameResumed;
        }

        private void OnDisable()
        {
            DisableInputs();
            GameManager.Instance.OnGamePaused -= OnGamePaused;
            GameManager.Instance.OnGameResumed -= OnGameResumed;
        }

        public void EnableInputs()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _inputActions.Enable();
        }

        public void DisableInputs()
        {
            Cursor.lockState = CursorLockMode.None;
            _inputActions.Disable();
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public T GetCustomComponent<T>() where T : Component
        {
            if (typeof(T) == typeof(MovementController))
            {
                return _movementController as T;
            }
            if (typeof(T) == typeof(AttackController))
            {
                return _attackController as T;
            }
            if (typeof(T) == typeof(PlayerHealthController)
                || typeof(T) == typeof(HealthComponent))
            {
                return _healthController as T;
            }
            return null;
        }

        public Transform GetDeathEnergyTargetTransform()
        {            
            return _deathEnergySphere;
        }

        public void AddDeathEnergy(float amount)
        {
            _attackController.ReloadAttack(AttackName.Portal, amount);            
        }

        public void OnGamePaused()
        {
            if (!TriggerOnGamePaused)
                return;
            DisableInputs();
        }

        public void OnGameResumed()
        {
            if (!TriggerOnGamePaused)
                return;
            EnableInputs();
        }
    }

}
