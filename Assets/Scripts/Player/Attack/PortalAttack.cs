using System;
using UnityEngine;
using UnityEngine.VFX;

namespace Player
{
    public class PortalAttack : BaseAttack
    {
        [SerializeField] private VisualEffect _visualEffect;

        [SerializeField] private float _damagePointsPerSecond;
        [SerializeField] private LayerMask _portalLayerMask;
        [SerializeField] private float _portalDetectionRadius = 10;
        private HealthComponent _currentPortal;

        public Action<HealthComponent> OnCurrentPortalChanged;

        protected override void Awake()
        {
            base.Awake();
            _visualEffect.enabled = false;
        }

        public override bool CanShoot()
        {
            return base.CanShoot() && _currentPortal != null;
        }

        public override void StartShooting()
        {
            base.StartShooting();

            if (!CanShoot())
                return;

            _visualEffect.enabled = true;
            _visualEffect.Play();
            OnShoot?.Invoke();
        }

        public override void StopShooting()
        {
            base.StopShooting();
            _visualEffect.Stop();
            _visualEffect.enabled = false;
        }

        public override void Shoot()
        {
            if (!CanShoot())
                return;

            _visualEffect.SetFloat("Reach", Mathf.Abs(Vector3.Distance(_visualEffect.transform.position, _currentPortal.transform.position) * 0.26f));//0.26f Adjustment to match the VFX impact position with the target

            if (_aimController.Target!=null && _aimController.Target.GetInstanceID() == _currentPortal.transform.GetInstanceID())
                _currentPortal.Damage(_damagePointsPerSecond * Time.deltaTime);

            if (MaxAmmunition == -1)
                return;

            DecreaseAmmo(Time.deltaTime * _damagePointsPerSecond);
            if (Ammunition <= 0)
            {
                SetAmmo(0);
                if (CurrentStatus != Status.Unavailable)
                    ChangeStatus(Status.Unavailable);
            }
        }

        protected override void Update()
        {
            base.Update();

            if (!IsShooting)
                return;


            _visualEffect.transform.position = _spawnPoint.position;
            _visualEffect.transform.rotation = _spawnPoint.rotation;
            Shoot();
        }

        protected override void FixedUpdate()
        {
            Collider[] colliders = new Collider[1];
            if (Physics.OverlapSphereNonAlloc(transform.position, _portalDetectionRadius, colliders, _portalLayerMask.value) > 0)
            {
                if (_currentPortal == null)
                {
                    _currentPortal = colliders[0].GetComponent<HealthComponent>();
                    OnCurrentPortalChanged?.Invoke(_currentPortal);
                    if (CurrentStatus != Status.Ready)
                        ChangeStatus(Status.Ready);
                }
            }
            else
            {
                if (CurrentStatus != Status.Unavailable)
                    ChangeStatus(Status.Unavailable);

                if (_currentPortal != null)
                {
                    _currentPortal = null;
                    OnCurrentPortalChanged?.Invoke(_currentPortal);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _portalDetectionRadius);
        }

    }
}
