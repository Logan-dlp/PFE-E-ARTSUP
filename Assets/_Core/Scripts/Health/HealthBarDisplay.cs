using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Health
{
    public class HealthBarDisplay : MonoBehaviour
    {
        [SerializeField] private HealthBarScriptableInt _healthBarScriptableInt;
        
        private Image _healthBar;

        private void Awake()
        {
            _healthBar = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _healthBarScriptableInt.HealthAmountAction += UpdateHealthBar;
        }

        private void OnDisable()
        {
            _healthBarScriptableInt.HealthAmountAction += UpdateHealthBar;
        }

        private void UpdateHealthBar(float healthAmount)
        {
            _healthBar.fillAmount = healthAmount;
        }
    }
}
