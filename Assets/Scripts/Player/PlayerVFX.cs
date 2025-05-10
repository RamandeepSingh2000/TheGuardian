using UnityEngine;
using UnityEngine.Rendering;

namespace Player
{
    public class PlayerVFX : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private PlayerHealthController _healthController;

        [Header("VFX")]
        [SerializeField] private Volume _postProcessVolume;
        [SerializeField] private ParticleSystem _healVFX;

        private void OnEnable()
        {
            _healthController.OnHealthRestored += OnHealthRestored;
            _healthController.OnDamageReceived += OnDamageReceived;
        }

        private void OnDisable()
        {
            _healthController.OnHealthRestored -= OnHealthRestored;
            _healthController.OnDamageReceived -= OnDamageReceived;
        }

        private void OnHealthRestored()
        {
            _healVFX.Play();
            StartCoroutine(Helper.PerformActionInSeconds(3, () => _healVFX.Stop()));
        }

        private void OnDamageReceived()
        {
            _postProcessVolume.enabled = true;
            StartCoroutine(Helper.PerformActionInSeconds(1, () => _postProcessVolume.enabled = false));
        }
    }
}
