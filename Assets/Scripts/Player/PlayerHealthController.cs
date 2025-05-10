using System;
using UnityEngine;

namespace Player
{
    public class PlayerHealthController : HealthComponent
    {
        public Action OnHealthRestored;
        public Action<float> OnHealthChanged;        

        public override void Damage(float points)
        {
            base.Damage(points);
            OnHealthChanged?.Invoke(HealthPoints);
        }

        public void Restore(float healthPoints)
        {
            if (HealthPoints == MaxHealth)
                return;

            HealthPoints += healthPoints;
            HealthPoints = Mathf.Clamp(HealthPoints, 0, MaxHealth);
            OnHealthChanged?.Invoke(HealthPoints);
            OnHealthRestored?.Invoke();
        }

    }
}