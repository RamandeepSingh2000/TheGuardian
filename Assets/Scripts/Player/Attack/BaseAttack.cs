using System;
using UnityEngine;

namespace Player
{
    public abstract class BaseAttack : MonoBehaviour
    {
        public enum Status
        {
            Ready,
            Loading,
            Unavailable
        }

        [SerializeField] protected AttackName _attackName;
        [SerializeField] protected Transform _spawnPoint;
        [SerializeField] protected AimController _aimController;

        [SerializeField, Tooltip("-1 for infinite ammo")] protected float _maxAmmunition = 1;
        [SerializeField, Tooltip("If -1 it starts with max ammunition")] private float _startAmmunition = -1;
        [SerializeField] protected float _cooldownTime = 1;
        [SerializeField] private bool _autoLoad = true;

        [SerializeField] protected Color _gemColor;

        public float MaxAmmunition => _maxAmmunition;
        public float Ammunition { get; protected set; }

        public AttackName AttackName => _attackName;

        public Status CurrentStatus { get; protected set; } = Status.Ready;

        public Color GemColor => _gemColor;

        public bool IsShooting { get; protected set; } = false;

        public Action<Status> OnStatusChanged;
        public Action OnStartShooting;
        public Action OnStopShooting;
        public Action OnShoot;

        public Action<float> OnAmmunitionChanged;

        public virtual float GetCooldownProgress() => 1;

        public abstract void Shoot();
        public virtual void StartShooting()
        {
            IsShooting = true;
            OnStartShooting?.Invoke();
        }
        public virtual void StopShooting()
        {
            IsShooting = false;
            OnStopShooting?.Invoke();
        }

        public virtual bool CanShoot()
        {
            return (_maxAmmunition == -1 || (int)Ammunition > 0) && CurrentStatus == Status.Ready;
        }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
            SetAmmo(_startAmmunition == -1 ? MaxAmmunition : _startAmmunition);
        }

        protected virtual void Update()
        {
            if (_autoLoad && MaxAmmunition != -1)
            {
                if (Ammunition == MaxAmmunition)
                    return;
                IncreaseAmmo(Time.deltaTime / _cooldownTime);
            }


            if (!IsShooting)
                return;

            if (_spawnPoint != null && _aimController != null)
                _spawnPoint.LookAt(_aimController.GetAimedPoint(), Vector3.up);
        }

        protected virtual void FixedUpdate()
        {
        }

        public virtual void ChangeStatus(Status status)
        {
            CurrentStatus = status;
            OnStatusChanged?.Invoke(CurrentStatus);
        }

        public virtual void SetAmmo(float ammo)
        {
            if (MaxAmmunition != -1)
                Ammunition = Mathf.Clamp(ammo, 0, MaxAmmunition);
            else Ammunition = -1;
            OnAmmunitionChanged?.Invoke(Ammunition);
        }

        public void IncreaseAmmo(float increase) => SetAmmo(Ammunition + increase);

        public void DecreaseAmmo(float decrease) => SetAmmo(Ammunition - decrease);

        public void ReloadFullAmmo() => SetAmmo(MaxAmmunition);
        public void SetStartAmmunition(int ammo) => _startAmmunition = ammo;
    }
}