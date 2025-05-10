using UnityEngine;

namespace Player
{
    public class FreezeAttack : BaseAttack
    {
        [SerializeField] private float _attackRadius = 10;
        [SerializeField] private float _freezeDuration = 5;
        [SerializeField] private LayerMask _enemiesLayerMask;
        [SerializeField] private ParticleSystem _freezeVFX;

        protected override void Start()
        {
            base.Start();
            _freezeVFX.transform.SetParent(null);
        }        

        public override void StartShooting()
        {
            base.StartShooting();
            Shoot();
        }

        public override void Shoot()
        {
            if (!CanShoot())
                return;

            var position = transform.position;
            position.y -= 1;
            _freezeVFX.transform.position = position;
            _freezeVFX.Play();
            Collider[] colliders = new Collider[5];
            int enemiesInArea = Physics.OverlapSphereNonAlloc(transform.position, _attackRadius, colliders, _enemiesLayerMask.value);
            if (enemiesInArea > 0)
            {
                for (int i = 0; i < enemiesInArea; i++)
                {
                    if (colliders[i].TryGetComponent(out EnemyStateMachineContext enemyStateMachineContext))
                    {
                        enemyStateMachineContext.Freeze();
                        StartCoroutine(Helper.PerformActionInSeconds(_freezeDuration, () => enemyStateMachineContext.UnFreeze()));
                    }
                }
            }
            DecreaseAmmo(1);
            OnShoot?.Invoke();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _attackRadius);
        }
    }
}