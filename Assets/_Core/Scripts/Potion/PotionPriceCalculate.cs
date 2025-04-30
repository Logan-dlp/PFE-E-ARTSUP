using UnityEngine;
using TMPro;

namespace MoonlitMixes.Potion
{
    public class PotionPriceCalculate : MonoBehaviour
    {
        public int SelectedPotionPrice { get; private set; }

        [SerializeField] private TextMeshProUGUI totalPriceText;
        private int totalPotionPrice = 0;

        public void SetSelectedPotionPrice(int price)
        {
            SelectedPotionPrice = price;
            Debug.Log($"SelectedPotionPrice mis à jour: {SelectedPotionPrice}");
        }

        public void CalculatePotionPrice(int basePrice, int failedAttempts)
        {
            if (basePrice <= 0)
            {
                Debug.Log("Aucune potion ou prix. Le prix est à 0.");
                UpdateTotalPriceUI();
                return;
            }

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
            return failedAttempts switch
            {
                0 => 1f,
                1 => 0.5f,
                2 => 0.25f,
                _ => 0f
            };
        }
    }
}
