using UnityEngine;
using UnityEngine.Pool;

namespace Player
{
    public class PoolableProjectileAttack : ProjectileAttack
    {
        [SerializeField] protected int _defaultCapacity = 10;
        [SerializeField] protected int _maxSize = 20;

        private IObjectPool<Projectile> _projectilePool;

        protected override void Awake()
        {
            base.Awake();
            //A projectile pool is created in order to reuse projectiles instances
            _projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck: true, _defaultCapacity, _maxSize);
        }

        /// <summary>
        /// Destroys a projectile instance from the pool in case it is above the maximum amount
        /// </summary>
        /// <param name="projectile"></param>
        private void OnDestroyPooledObject(Projectile projectile)
        {
            Destroy(projectile.gameObject);
        }

        /// <summary>
        /// Deactivates the projectile gameobject once it's put into the pool
        /// </summary>
        /// <param name="projectile"></param>
        private void OnReleaseToPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
        }

        /// <summary>
        /// Activates the projectile gameobject once it's released from the pool
        /// </summary>
        /// <param name="projectile"></param>
        private void OnGetFromPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        /// <summary>
        /// Instantiates the projectile and updates its references
        /// </summary>
        /// <returns></returns>
        protected override Projectile GetNextBullet()
        {
            return _projectilePool.Get();
        }

        private Projectile CreateProjectile()
        {
            PoolableProjectile instance = (PoolableProjectile)Instantiate(_projectilePrefab);
            instance.ObjectPool = _projectilePool;
            return instance;
        }
        
    }
}