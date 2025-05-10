using UnityEngine;
using UnityEngine.UI;

namespace Player.UI
{
    public class AttackButton : MonoBehaviour
    {
        [SerializeField] private AttackName _attackName;
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Image _loadImage;
        [SerializeField] private CanvasGroup _canvasGroup;

        public AttackName AttackName=>_attackName;

        public void Enable() => _canvasGroup.alpha = 1;

        public void Disable() => _canvasGroup.alpha = 0.3f;

        public void Select() => _toggle.isOn = true;

        public void SetProgress(float percentage) => _loadImage.fillAmount = percentage;
        
    }
}