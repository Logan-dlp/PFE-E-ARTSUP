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


        private string _selectedPotionName;
        public string SelectedPotionName => _selectedPotionName;

        public static event Action<string> OnPotionChoiceSelected;

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
                _selectedPotionName = _potionInventory.PotionList[0].Recipe.RecipeName;
                Debug.Log($"Potion choisie par le PNJ : {_selectedPotionName}");

                if (_potionPrices.TryGetValue(_selectedPotionName, out int price))
                {
                    Debug.Log($"Potion confirm�e: {_selectedPotionName}, Prix: {price}");
                    _potionPrices.Remove(_selectedPotionName);

                    if (_potionPriceCalculated != null)
                    {
                        _potionPriceCalculated.SetSelectedPotionPrice(price);
                    }
                    _potionPrices[_selectedPotionName] = _potionInventory.PotionList[0].Price;
                }
            }

            _potionInventory.UpdatePotionCanvas();
        }

        public void SelectPotion(string potionName)
        {
            if (_selectedPotionName == potionName)
            {
                Debug.Log("Bonne potion choisie !");
            }
            else
            {
                Debug.Log(string.IsNullOrEmpty(potionName) ? "Pas de potion choisie !" : "Mauvaise potion, essayez encore !");
            }

            OnPotionChoiceSelected?.Invoke(potionName);
            _potionChoicePanel.SetActive(false);
        }
    }
}