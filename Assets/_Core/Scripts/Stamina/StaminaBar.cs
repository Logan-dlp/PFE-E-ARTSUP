using UnityEngine;
using UnityEngine.UI;
using MoonlitMixes.Player;

namespace MoonlitMixes.Stamina
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement; 
        [SerializeField] private Image staminaBarImage;
        private void Start()
        {
            if (playerMovement == null)
            {
                playerMovement = GetComponentInParent<PlayerMovement>();
            }
            if (staminaBarImage == null)
            {
                staminaBarImage = GetComponent<Image>();
            }
        }

        private void Update()
        {
            if (playerMovement != null && staminaBarImage != null)
            {
                float staminaPercentage = playerMovement.GetCurrentStamina() / playerMovement.GetMaxStamina();
                staminaBarImage.fillAmount = staminaPercentage;
            }
        }
    }
}