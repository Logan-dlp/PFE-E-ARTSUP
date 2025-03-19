using UnityEngine;
using TMPro;

namespace MoonlitMixes.Potion
{
    public class PotionPriceCalculate : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI totalPriceText;
        public int SelectedPotionPrice { get; private set; }
        private int totalPotionPrice = 0;

        public void SetSelectedPotionPrice(int price)
        {
            SelectedPotionPrice = price;
            Debug.Log($"SelectedPotionPrice mis à jour: {SelectedPotionPrice}");
        }

        public void CalculatePotionPrice(int basePrice, int failedAttempts)
        {
            float multiplier = GetMultiplier(failedAttempts);
            int calculatedPrice = Mathf.FloorToInt(basePrice * multiplier);

            totalPotionPrice += calculatedPrice;

            UpdateTotalPriceUI();

            Debug.Log($"Prix calculé avec multiplicateur {multiplier}: {calculatedPrice}, Total accumulé: {totalPotionPrice}");
        }

        private void UpdateTotalPriceUI()
        {
            if (totalPriceText != null)
            {
                totalPriceText.text = $"Total: {totalPotionPrice}";
            }
        }

        private float GetMultiplier(int failedAttempts)
        {
            return failedAttempts == 0 ? 1f : failedAttempts == 1 ? 0.5f : failedAttempts == 2 ? 0.25f : 0f;
        }
    }
}