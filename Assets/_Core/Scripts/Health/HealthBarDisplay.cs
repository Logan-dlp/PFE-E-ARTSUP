using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Health
{
    public class HealthBarDisplay : MonoBehaviour
    {
        [SerializeField] private HealthBarScriptableInt _healthBarScriptableInt;
        [SerializeField] private Image _healthBar;

        private void OnEnable()
        {
            _healthBarScriptableInt.HealthAmountAction += UpdateHealthBar;
        }

        private void OnDisable()
        {
            _healthBarScriptableInt.HealthAmountAction -= UpdateHealthBar;
        }

        private void UpdateHealthBar(float healthAmount)
        {
            if (_healthBar != null)
            {
                _healthBar.fillAmount = healthAmount;
            }
        }
    }
}
