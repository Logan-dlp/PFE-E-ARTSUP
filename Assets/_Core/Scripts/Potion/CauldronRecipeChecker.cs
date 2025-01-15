using System.Collections.Generic;
using MoonlitMixes.Item;
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
                Debug.LogError("CauldronTimer n'est pas attach� au chaudron !");
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
                Debug.LogWarning("L'ingr�dient ajout� est null !");
                return;
            }

            if (!_cauldronTimer.CanAddItem())
            {
                Debug.Log("Il faut attendre avant d'ajouter un autre ingr�dient !");
                return;
            }

            Debug.Log($"Ingr�dient ajout� : {ingredient.ObjectName}");

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
                            Debug.LogWarning($"Trop d'ingr�dients du type {ingredient.Type} ajout�s !");
                            return false;
                        }

                        return true;
                    }
                }
            }

            Debug.LogWarning($"L'ingr�dient {ingredient.ObjectName} ne correspond � aucune recette !");
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