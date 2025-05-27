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

        private Dictionary<string, int> _potionPrices = new Dictionary<string, int>();
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

        public void ShowPotionChoices()
        {
            _potionChoicePanel.SetActive(true);

            if (_potionInventory.PotionList.Count > 0)
            {
                _selectedPotionResult = _potionInventory.PotionList[0];
                Debug.Log($"Potion choisie par le PNJ : {_selectedPotionResult}");

                if (_potionPrices.TryGetValue(_selectedPotionResult.Recipe.RecipeName, out int price))
                {
                    Debug.Log($"Potion confirm�e: {_selectedPotionResult}, Prix: {price}");
                    _potionPrices.Remove(_selectedPotionResult.Recipe.RecipeName);

                    if (_potionPriceCalculated != null)
                    {
                        _potionPriceCalculated.SetSelectedPotionPrice(price);
                    }
                    _potionPrices[_selectedPotionResult.Recipe.RecipeName] = _potionInventory.PotionList[0].Price;
                }
            }

            _potionInventory.UpdatePotionCanvas();
        }

        public void SelectPotion(PotionResult potionResult)
        {
            if (_selectedPotionResult == potionResult)
            {
                Debug.Log("Bonne potion choisie !");
            }
            else
            {
                Debug.Log(potionResult == null ? "Pas de potion choisie !" : "Mauvaise potion, essayez encore !");
            }

            OnPotionChoiceSelected?.Invoke(potionResult);
            _potionChoicePanel.SetActive(false);
        }
    }
}