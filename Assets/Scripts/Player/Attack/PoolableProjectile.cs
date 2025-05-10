using UnityEngine;
using UnityEngine.Pool;

namespace Player
{
    public class PoolableProjectile : Projectile
    {
        private IObjectPool<Projectile> _objectPool;
        public IObjectPool<Projectile> ObjectPool { set => _objectPool = value; }       

        /// <summary>
        /// Places the instance into the pool
        /// </summary>
        protected override void Deactivate()
        {            
            _rigidbody.linearVelocity = Vector3.zero;
            _objectPool.Release(this);
        }
    }
}