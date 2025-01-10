using System.Collections.Generic;
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

        private void Awake()
        {
            _cauldronTimer = GetComponent<CauldronTimer>();
            if (_cauldronTimer == null)
            {
                Debug.LogError("CauldronTimer n'est pas attaché au chaudron !");
            }
        }

        public void TogleShowInteractivity()
        {
            _isActive = !_isActive;
            _interactUI.SetActive(_isActive);
        }

        public void AddIngredient(ItemData ingredient)
        {
            if (ingredient == null)
            {
                Debug.LogWarning("L'ingrédient ajouté est null !");
                return;
            }

            if (!_cauldronTimer.CanAddItem())
            {
                Debug.Log("Il faut attendre avant d'ajouter un autre ingrédient !");
                return;
            }

            Debug.Log($"Ingrédient ajouté : {ingredient.ObjectName}");

            _currentIngredients.Add(ingredient);
            _cauldronTimer.ResetCooldown();
            TriggerBubbleVFX();

            bool isValid = ValidateIngredient(ingredient);

            if (!isValid)
            {
                HandleFailedPotion();
                return;
            }

            CheckRecipeCompletion();
        }

        private bool ValidateIngredient(ItemData ingredient)
        {
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
                            Debug.LogWarning($"Trop d'ingrédients du type {ingredient.Type} ajoutés !");
                            return false;
                        }

                        return true;
                    }
                }
            }

            Debug.LogWarning($"L'ingrédient {ingredient.ObjectName} ne correspond à aucune recette !");
            return false;
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
            Debug.Log($"Recette réussie : {recipe.RecipeName} !");
            _currentIngredients.Clear();
        }

        private void HandleFailedPotion()
        {
            Debug.Log("Potion ratée !");
            _currentIngredients.Clear();
        }

        private void TriggerBubbleVFX()
        {
            if (_bubbleVFX != null)
            {
                _bubbleVFX.SetActive(true);
                Invoke(nameof(DisableBubbleVFX), _cauldronTimer.GetTimeRemaining());
            }
        }

        private void DisableBubbleVFX()
        {
            if (_bubbleVFX != null)
            {
                _bubbleVFX.SetActive(false);
            }
        }
    }
}