using MoonlitMixes.Potion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoonlitMixes.Dialogue
{
    public class PotionChoiceController : MonoBehaviour
    {
        [SerializeField] private GameObject _potionChoicePanel;
        [SerializeField] private PotionInventory _potionInventory;

        private PotionResult[] _requestedPotions;
        private Dictionary<string, int> _potionPrices = new();
        private PotionPriceCalculate _potionPriceCalculated;

        private PotionResult _selectedPotionResult;
        public PotionResult SelectedPotionResult => _selectedPotionResult;

        public static event Action<PotionResult> OnPotionChoiceSelected;

        private void Awake()
        {
            _potionPriceCalculated = FindFirstObjectByType<PotionPriceCalculate>();
        }

        private void Start()
        {
            _potionChoicePanel.SetActive(false);
        }

        public void SetRequestedPotions(PotionResult[] requestedPotions)
        {
            _requestedPotions = requestedPotions;
        }

        public void ShowPotionChoices()
        {
            _potionChoicePanel.SetActive(true);

            if (_requestedPotions != null && _requestedPotions.Length > 0)
            {
                _selectedPotionResult = _requestedPotions[0];
                Debug.Log($"Potion assignée au PNJ : {_selectedPotionResult}");

                if (_potionPrices.TryGetValue(_selectedPotionResult.Recipe.RecipeName, out int price))
                {
                    Debug.Log($"Potion confirmée: {_selectedPotionResult}, Prix: {price}");
                    _potionPrices.Remove(_selectedPotionResult.Recipe.RecipeName);

                    _potionPriceCalculated?.SetSelectedPotionPrice(price);
                    _potionPrices[_selectedPotionResult.Recipe.RecipeName] = _selectedPotionResult.Price;
                }
            }
            else
            {
                Debug.LogWarning("Aucune potion assignée au PNJ.");
                _selectedPotionResult = null;
            }

            _potionInventory.UpdatePotionCanvas();
        }

        public void SelectPotion(PotionResult potionResult)
        {
            _selectedPotionResult = potionResult;

            Debug.Log($"Potion sélectionnée : {(potionResult != null ? potionResult.Recipe.RecipeName : "aucune")}");

            OnPotionChoiceSelected?.Invoke(potionResult);
            _potionChoicePanel.SetActive(false);
        }
    }
}