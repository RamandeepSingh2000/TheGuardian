using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class CrosshairController : MonoBehaviour
    {
        [SerializeField] private Image _crosshairIcon;
        [SerializeField] private AimController _aimController;

        private void Start()
        {
            _aimController.OnTargetEnter += () => _crosshairIcon.color = Color.red;
            _aimController.OnTargetExit += () => _crosshairIcon.color = Color.white;
        }        
    }
}