using System;
using UnityEngine;

namespace Player
{
    public class AimController : MonoBehaviour
    {
        [SerializeField] private Transform _aimOrigin;
        [SerializeField] private LayerMask _targetLayers;
        [SerializeField] private float _maxDistance = 100.0f;
        private Vector3 _aimPointPosition;
        private bool _isAimingAtTarget = false;
        public Transform Target { get; private set; } = null;

        public Action OnTargetEnter;
        public Action OnTargetExit;

        private void FixedUpdate()
        {
            RaycastHit hit;
            if (Physics.Raycast(_aimOrigin.position, _aimOrigin.forward, out hit, _maxDistance, _targetLayers))
            {
                _aimPointPosition = hit.point;
                if (!_isAimingAtTarget)
                {
                    OnTargetEnter?.Invoke();
                    Target=hit.transform;
                    _isAimingAtTarget = true;
                }
            }
            else
            {
                if (_isAimingAtTarget)
                {
                    OnTargetExit?.Invoke();
                    _isAimingAtTarget = false;
                    Target = null;
                }
                _aimPointPosition = _aimOrigin.position + _aimOrigin.forward * _maxDistance;
            }
        }

        public Vector3 GetAimedPoint() => _aimPointPosition;
    }
}