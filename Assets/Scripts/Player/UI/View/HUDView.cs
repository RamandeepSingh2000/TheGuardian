using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UI
{
    public class HUDView : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private Image _healthBarImage;

        [Header("Attack")]
        [SerializeField] private AttackButton[] _powerIcons;
        [SerializeField] private DeathEnergyHolder _deathEnergyHolder;
        [SerializeField] private TMP_Text _deathEnergyText;

        [Header("Portals")]
        [SerializeField] private Image _portalLoadingImage;
        [SerializeField] private TMP_Text _portalsText;

        [Header("Enemy")]
        [SerializeField] private TMP_Text _killedEnemiesText;

        private void Start()
        {
            SetNumberOfEnemiesKilled(0);
        }

        public void UpdateHealth(float percentage) => _healthBarImage.fillAmount = percentage;

        public void UpdateDeathEnergyPercentage(float percentage)
        {
            _deathEnergyHolder.SetFillPercent(percentage);
            _deathEnergyText.text = $"{(int)(percentage * 100f)}%";
        }

        #region ATTACK

        public void EnablePower(AttackName attackName)
        {
            GetPowerIcon(attackName).Enable();
        }

        public void DisablePower(AttackName attackName)
        {
            GetPowerIcon(attackName).Disable();
        }

        public void SelectPower(AttackName attackName)
        {
            GetPowerIcon(attackName).Select();
        }

        public void SetPowerAmmunition(AttackName attackName, float percentage) => GetPowerIcon(attackName).SetProgress(percentage);

        private AttackButton GetPowerIcon(AttackName attackName) => _powerIcons.First(a => a.AttackName.Equals(attackName));

        #endregion

        #region PORTALS

        public void SetPortalsStats(int active, int total) => _portalsText.text = $"{active} / {total}";

        public void SetPortalLifePercentage(float percentage) => _portalLoadingImage.fillAmount = percentage;

        #endregion

        public void SetNumberOfEnemiesKilled(int number) => _killedEnemiesText.text = "x " + number.ToString();
    }
}
