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
            int calculatedPrice = 0;

            if (basePrice <= 0)
            {
                Debug.Log("Aucune potion ou prix. Le prix est à 0.");
            }
            else
            {
                float multiplier = GetMultiplier(failedAttempts);
                calculatedPrice = Mathf.FloorToInt(basePrice * multiplier);
                totalPotionPrice += calculatedPrice;

                Debug.Log($"Prix calculé avec multiplicateur {multiplier}: {calculatedPrice}, Total accumulé: {totalPotionPrice}");
            }

            // Toujours mettre à jour l'UI, même si basePrice est 0
            UpdateTotalPriceUI();
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