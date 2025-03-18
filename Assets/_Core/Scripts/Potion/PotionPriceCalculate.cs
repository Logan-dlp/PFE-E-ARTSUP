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

        public void CalculatePotionPrice(int basePrice, float multiplier)
        {
            int calculatedPrice = Mathf.FloorToInt(basePrice * multiplier);

            totalPotionPrice += calculatedPrice;

            UpdateTotalPriceUI();

            Debug.Log($"Prix calculé avec multiplicateur {multiplier}: {calculatedPrice}");
        }

        private void UpdateTotalPriceUI()
        {
            if (totalPriceText != null)
            {
                totalPriceText.text = $"Total: {totalPotionPrice}";
            }
        }
    }
}