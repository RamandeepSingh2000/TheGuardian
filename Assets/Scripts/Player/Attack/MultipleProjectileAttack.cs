using UnityEngine;

namespace Player
{
    public class MultipleProjectileAttack : PoolableProjectileAttack
    {
        [SerializeField] private float _enemyDetectionRadius = 10;
        [SerializeField] private LayerMask _enemiesLayerMask;

        public override void StartShooting()
        {
            base.StartShooting();
            Shoot();
        }

        public override void Shoot()
        {
            if (!CanShoot())
                return;

            var enemies = GetNearbyTargets();
            if (enemies == null)
                return;

            foreach (var enemy in enemies)
            {
                TargetedProjectile projectile = GetNextBullet() as TargetedProjectile;
                if (projectile == null)
                    return;

                projectile.transform.position = _spawnPoint.position;
                projectile.transform.LookAt(enemy.GetComponent<Collider>().bounds.center);
                projectile.SetTarget(enemy);
                projectile.Shoot();
                projectile.StartDeactivationTimer();
            }
            DecreaseAmmo(1);
            OnShoot?.Invoke();

        }

        private Transform[] GetNearbyTargets()
        {
            Collider[] colliders = new Collider[5];
            int detectedEnemies = Physics.OverlapSphereNonAlloc(transform.position, _enemyDetectionRadius, colliders, _enemiesLayerMask.value);
            if (detectedEnemies > 0)
            {                
                Transform[] enemiesTransform = new Transform[detectedEnemies];
                for (int i = 0; i < detectedEnemies; i++)
                    //enemiesTransform[i] = colliders[i].bounds.center;
                    enemiesTransform[i] = colliders[i].transform;
                return enemiesTransform;
            }
            return null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _enemyDetectionRadius);
        }
    }
}
