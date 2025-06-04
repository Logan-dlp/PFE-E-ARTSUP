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

        /// <summary>
        /// Affiche la bonne potion demandée par le client selon l'index courant.
        /// </summary>
        /// <param name="currentPotionIndex">Index de la potion demandée</param>
        public void ShowPotionChoices(int currentPotionIndex)
        {
            _potionChoicePanel.SetActive(true);

            if (_requestedPotions != null && _requestedPotions.Length > currentPotionIndex)
            {
                _selectedPotionResult = _requestedPotions[currentPotionIndex];

                Debug.Log($"[PNJ] Potion demandée (étape {currentPotionIndex + 1}) : {_selectedPotionResult.Recipe.RecipeName}, Prix : {_selectedPotionResult.Price}");

                // Enregistrer ou recalculer le prix si nécessaire
                if (_potionPrices.TryGetValue(_selectedPotionResult.Recipe.RecipeName, out int price))
                {
                    _potionPrices.Remove(_selectedPotionResult.Recipe.RecipeName);
                }

                _potionPrices[_selectedPotionResult.Recipe.RecipeName] = _selectedPotionResult.Price;
                _potionPriceCalculated?.SetSelectedPotionPrice(_selectedPotionResult.Price);
            }
            else
            {
                Debug.LogWarning("[PNJ] Aucune potion disponible à cet index.");
                _selectedPotionResult = null;
            }

            _potionInventory.UpdatePotionCanvas();
        }

        /// <summary>
        /// Appelé lorsque le joueur sélectionne une potion via l'UI.
        /// </summary>
        public void SelectPotion(PotionResult potionResult)
        {
            _selectedPotionResult = potionResult;

            if (potionResult != null)
            {
                Debug.Log($"[Joueur] Potion sélectionnée : {potionResult.Recipe.RecipeName}, Prix : {potionResult.Price}");
            }
            else
            {
                Debug.LogWarning("Aucune potion sélectionnée.");
            }

            OnPotionChoiceSelected?.Invoke(potionResult);
            _potionChoicePanel.SetActive(false);
        }
    }
}