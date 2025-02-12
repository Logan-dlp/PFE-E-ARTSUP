using System.Collections.Generic;
using System.Linq;
using MoonlitMixes.Item;
using MoonlitMixes.Player;
using MoonlitMixes.Potion;
using UnityEngine;

namespace MoonlitMixes.CookingMachine
{
    public class CauldronRecipeChecker : MonoBehaviour
    {
        [SerializeField] private GameObject _interactUI;
        [SerializeField] private GameObject _bubbleVFX;
        [SerializeField] private List<Recipe> _allRecipes;
        [SerializeField] private List<ItemData> _currentIngredients = new List<ItemData>();

        private CauldronTimer _cauldronTimer;
        private bool _isActive = false;
        private PotionInventory _potionInventory;
        private CauldronMixing _cauldronMixing;
        private bool _qteSuccess;
        private ItemData _lastAddedItem;
        private bool _canMix;

        public bool CanMix
        {
            get => _canMix;
            set => _canMix = value;
        }

        private void Awake()
        {
            _cauldronMixing = GetComponent<CauldronMixing>();
            _cauldronTimer = GetComponent<CauldronTimer>();
            _potionInventory = FindFirstObjectByType<PotionInventory>();

            if (_cauldronTimer == null)
            {
                Debug.LogError("CauldronTimer n'est pas attach� au chaudron !");
            }
        }

        public void TogleShowInteractivity()
        {
            _isActive = !_isActive;
            //_interactUI.SetActive(_isActive);
        }

        public void AddIngredient(ItemData ingredient)
        {
            if(!_currentIngredients.Any()) _cauldronTimer.TimerIsActive = true;

            if (ingredient == null)
            {
                Debug.LogWarning("L'ingr�dient ajout� est null !");
                return;
            }

            
            if (!_cauldronTimer.CanAction())
            {
                Debug.Log("Il faut attendre avant d'ajouter un autre ingr�dient !");
                return;
            }

            Debug.Log("Add + " + ingredient);
            Debug.Log($"Ingr�dient ajout� : {ingredient.ObjectName}");
            _currentIngredients.Add(ingredient);
            _lastAddedItem = ingredient;
            _cauldronTimer.ResetCooldown();
            TriggerBubbleVFX();
        }

        private void ValidateIngredient(ItemData ingredient)
        {
            if (!_qteSuccess)
            {
                HandleFailedPotion();
                return;
            }

            Dictionary<ElementType, int> currentCounts = new Dictionary<ElementType, int>();
            foreach (var ing in _currentIngredients)
            {
                if (!currentCounts.ContainsKey(ing.Type))
                {
                    currentCounts[ing.Type] = 0;
                }
                currentCounts[ing.Type]++;
            }

            foreach (Recipe recipe in _allRecipes)
            {
                foreach (var requirement in recipe.RequiredIngredients)
                {
                    if (requirement.ElementType == ingredient.Type)
                    {
                        currentCounts.TryGetValue(ingredient.Type, out int currentCount);

                        if (currentCount > requirement.Quantity)
                        {
                            Debug.LogWarning($"Trop d'ingr�dients du type {ingredient.Type} ajout�s !");
                            return;
                        }
                        _cauldronTimer.ResetCooldown();
                        CheckRecipeCompletion();
                    }
                }
            }
        }

        private void CheckRecipeCompletion()
        {
            foreach (Recipe recipe in _allRecipes)
            {
                if (IsRecipeComplete(recipe))
                {
                    HandleSuccessfulPotion(recipe);
                    return;
                }
            }
        }

        private bool IsRecipeComplete(Recipe recipe)
        {
            Dictionary<ElementType, int> ingredientCount = new Dictionary<ElementType, int>();
            foreach (var ingredient in _currentIngredients)
            {
                if (!ingredientCount.ContainsKey(ingredient.Type))
                {
                    ingredientCount[ingredient.Type] = 0;
                }
                ingredientCount[ingredient.Type]++;
            }

            foreach (var requirement in recipe.RequiredIngredients)
            {
                if (!ingredientCount.TryGetValue(requirement.ElementType, out int count) || count < requirement.Quantity)
                {
                    return false;
                }
            }

            return true;
        }

        private void HandleSuccessfulPotion(Recipe recipe)
        {
            _potionInventory.PotionList.Add(recipe.Potion);
            Debug.Log($"Recette r�ussie : {recipe.RecipeName} !");
            _currentIngredients.Clear();
        }

        private void HandleFailedPotion()
        {
            Debug.Log("Potion rat�e !");
            _currentIngredients.Clear();
        }

        private void TriggerBubbleVFX()
        {
            if (_bubbleVFX != null)
            {
                _bubbleVFX.SetActive(true);
                Invoke(nameof(DisableBubbleVFX), _cauldronTimer.RemainingTime);
            }
        }

        private void DisableBubbleVFX()
        {
            if (_bubbleVFX != null)
            {
                _bubbleVFX.SetActive(false);
            }
        }

        public void CheckQTE(bool state)
        {
            _qteSuccess = state;
            ValidateIngredient(_lastAddedItem);
        }

        public void Mix(PlayerInteraction playerInteraction)
        {
            _cauldronMixing.ConvertItem(playerInteraction);
        }
    }
}