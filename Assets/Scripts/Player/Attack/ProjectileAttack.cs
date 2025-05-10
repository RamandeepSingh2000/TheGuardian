using UnityEngine;

namespace Player
{
    public class ProjectileAttack : BaseAttack
    {
        [Tooltip("Prefab to shoot")]
        [SerializeField] protected Projectile _projectilePrefab;

        [SerializeField] protected float _fireRate = 0.5f;

        protected float _timeToNextShoot = 0;

        protected virtual Projectile GetNextBullet()
        {
            return Instantiate(_projectilePrefab);
        }

        /// <summary>
        /// Retrieves a projectile from the pool, positions it at the spawn position and shoots it
        /// as long as the fire rate allows it
        /// </summary>        
        public override void Shoot()
        {
            if (!CanShoot())
                return;

            Projectile projectile = GetNextBullet();

            if (projectile == null)
                return;

            projectile.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            projectile.Shoot();
            projectile.StartDeactivationTimer();
            _timeToNextShoot = 0;
            OnShoot?.Invoke();
        }

        protected override void Update()
        {
            base.Update();

            if (_timeToNextShoot < _fireRate)
                _timeToNextShoot += Time.deltaTime;

            if (!IsShooting)
                return;

            Shoot();
        }

        public override bool CanShoot()
        {
            return base.CanShoot() && _timeToNextShoot >= _fireRate;
        }
    }
}