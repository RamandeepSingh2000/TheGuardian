using UnityEngine;

namespace Player
{
    public class HealthItem : MonoBehaviour
    {
        [SerializeField] private float _healthPoints = 40;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerHealthController health) && !health.HasMaxHealth())
            {
                health.Restore(_healthPoints);
                Destroy(gameObject);
            }                
        }        
    }
}
