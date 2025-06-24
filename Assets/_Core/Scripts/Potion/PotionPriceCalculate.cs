using UnityEngine;
using TMPro;

namespace MoonlitMixes.Potion
{
    public class PotionPriceCalculate : MonoBehaviour
    {
        public int SelectedPotionPrice { get; private set; }

        [SerializeField] private TextMeshProUGUI totalPriceText;
        [SerializeField] private int totalPotionPrice = 0;
        [SerializeField] private bool _isLoanRefunded = false;
        [SerializeField] private int _loanPrice = 600;
        [SerializeField] private int _day1NeededMoney = 120;
        [SerializeField] private int _day2NeededMoney = 220;
        [SerializeField] private int _day3NeededMoney = 415;
        private int _day1Money;
        private int _day2Money;
        private int _day3Money;

        public int Day1NeededMoney => _day1NeededMoney;
        public int Day2NeededMoney => _day2NeededMoney;
        public int Day3NeededMoney => _day3NeededMoney;
        public int Day1Money => _day1Money;
        public int Day2Money => _day2Money;
        public int Day3Money => _day3Money;
        public bool IsLoanRefunded =>_isLoanRefunded;

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
            _isLoanRefunded = VerficationLoan();
        }

        
        private void CalculateDailyMoney(int day)
        {
            switch (day)
            {
                case 1:
                    _day1Money = totalPotionPrice;
                    break;
                case 2:
                    _day2Money = totalPotionPrice- _day1Money;
                    break;
                case 3:
                    _day3Money = totalPotionPrice - (_day1Money+_day2Money);
                    break;
                default:
                    Debug.LogWarning("Mauvaise valeure de jour envoyé");
                    break; 
            }
        }
        private void UpdateTotalPriceUI()
        {
            if (totalPriceText != null)
            {
                totalPriceText.text = $"Total: {totalPotionPrice} / {_loanPrice}";
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
        private bool VerficationLoan()
        {
            if (totalPotionPrice>_loanPrice) return true;
            else return false;
        }
    }
}