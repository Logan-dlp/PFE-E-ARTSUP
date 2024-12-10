using System.Collections.Generic;
using UnityEngine;

namespace MoonlitMixes.CookingMachine
{
    public class CauldronMixing : ACookingMachine
    {
        [SerializeField] private GameObject _interactUI;
        [SerializeField] private List<Recipe> _allRecipes;
        [SerializeField] private List<ItemData> _currentIngredients = new List<ItemData>(); // Ingrédients ajoutés
        private bool _isActive = false;

        public override void TogleShowInteractivity()
        {
            _isActive = !_isActive;
            _interactUI.SetActive(_isActive);
        }

        public void AddIngredient(ItemData ingredient)
        {
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
            foreach (Recipe recipe in _allRecipes)
            {
                foreach (var requirement in recipe.RequiredIngredients)
                {
                    if (requirement.ElementType == ingredient.Type)
                    {
                        return true;
                    }
                }
            }

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