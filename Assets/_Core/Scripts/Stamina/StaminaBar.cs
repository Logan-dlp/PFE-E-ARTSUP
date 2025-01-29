using UnityEngine;
using UnityEngine.UI;
using MoonlitMixes.Player;

namespace MoonlitMixes.Stamina
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement; 
        [SerializeField] private Image _staminaBarImage;
        private void Start()
        {
            if (_playerMovement == null)
            {
                _playerMovement = GetComponentInParent<PlayerMovement>();
            }
            if (_staminaBarImage == null)
            {
                _staminaBarImage = GetComponent<Image>();
            }
        }

        private void Update()
        {
            if (_playerMovement != null && _staminaBarImage != null)
            {
                float staminaPercentage = _playerMovement.GetCurrentStamina() / _playerMovement.GetMaxStamina();
                _staminaBarImage.fillAmount = staminaPercentage;
            }
        }
    }
}