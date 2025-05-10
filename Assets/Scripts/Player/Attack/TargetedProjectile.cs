using UnityEngine;

namespace Player
{
    public class TargetedProjectile : PoolableProjectile
    {
        private Transform _target;

        public void SetTarget(Transform target) => _target = target;

        protected override void OnTriggerEnter(Collider other)
        {
            if (_target != null && other.transform.GetInstanceID() == _target.GetInstanceID() && other.TryGetComponent(out HealthComponent target))
            {
                HitTarget(target);
                _target = null;
                Deactivate();
            }
        }
    }
}
