using UnityEngine;
using UnityEngine.UI;
using MoonlitMixes.Player;

namespace MoonlitMixes.Stamina
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Image _staminaBarImage;

        private void Awake()
        {
            if (_staminaBarImage == null)
            {
                _staminaBarImage = GetComponent<Image>();
            }
        }

        private void OnEnable()
        {
            if (_playerMovement == null)
            {
                _playerMovement = FindObjectOfType<PlayerMovement>();
            }

            if (_playerMovement != null)
            {
                _playerMovement.OnStaminaChanged += UpdateStaminaBar;
            }
        }

        private void OnDisable()
        {
            if (_playerMovement != null)
            {
                _playerMovement.OnStaminaChanged -= UpdateStaminaBar;
            }
        }

        private void UpdateStaminaBar(float staminaPercentage)
        {
            _staminaBarImage.fillAmount = staminaPercentage;
        }
    }
}