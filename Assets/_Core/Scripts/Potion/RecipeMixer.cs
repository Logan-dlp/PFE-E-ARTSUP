using System.Collections.Generic;
using UnityEngine;

namespace MoonlitMixes.CookingMachine
{
    public class RecipeMixer : MonoBehaviour
    {
        [SerializeField] private GameObject _interactUI;
        [SerializeField] private List<Recipe> _allRecipes;
        [SerializeField] private List<ItemData> _currentIngredients = new List<ItemData>();
        private bool _isActive = false;

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

            Debug.Log($"Ingrédient ajouté : {ingredient.ObjectName}");

            _currentIngredients.Add(ingredient);

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
                        if (requirement.State != ingredient.State)
                        {
                            Debug.LogWarning($"L'état de l'ingrédient {ingredient.ObjectName} ne correspond pas à l'état requis pour la recette !");
                            return false;
                        }

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
    }
}