using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        protected Rigidbody _rigidbody;
        [Header("Properties")]
        [SerializeField] protected float _speed = 20f;
        [SerializeField] protected float _timeOutDelay = 3f;
        [SerializeField] protected int _damagePoints = 10;
        protected bool _isMoving = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            if (_isMoving)
                _rigidbody.linearVelocity = transform.forward * _speed * Time.fixedDeltaTime;
        }

        /// <summary>
        /// Applies a force to the rigidbody to move forward
        /// </summary>
        public virtual void Shoot()
        {
            _isMoving=true;            
        }

        /// <summary>
        /// Triggers the HitTarget method from the parent's class as long as it hit 
        /// an object that implements the IHealth interface
        /// </summary>
        /// <param name="other"></param>
        protected virtual void OnTriggerEnter(Collider other)
        {            
            if (other.TryGetComponent(out HealthComponent target))
                HitTarget(target);

            Deactivate();
        }

        /// <summary>
        /// Starts a timer to automatically deactivate the projectile
        /// </summary>
        public void StartDeactivationTimer()
        {
            StartCoroutine(DeactivationCoroutine());
        }

        /// <summary>
        /// Waits for some seconds to deactivate the instance in case it didn't collide with
        /// an enemy
        /// </summary>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator DeactivationCoroutine()
        {
            yield return new WaitForSeconds(_timeOutDelay);
            Deactivate();
        }

        /// <summary>
        /// Places the instance into the pool
        /// </summary>
        protected virtual void Deactivate()
        {
            Destroy(this.gameObject);
        }

        /// <summary>
        /// Damages the target
        /// </summary>
        /// <param name="target"></param>
        protected virtual void HitTarget(HealthComponent target)
        {
            target.Damage(_damagePoints);
        }
    }
}