using MoonlitMixes.CookingMachine;
using MoonlitMixes.Datas;
using MoonlitMixes.Item;
using MoonlitMixes.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Potion
{
    public class CauldronRecipeChecker : MonoBehaviour
    {
        public event Action OnStirStarted;
        public event Action OnStirEnded;

        [Header("Recipe Configuration")]
        [Tooltip("Liste de toutes les recettes possibles à vérifier.")]
        [SerializeField] private List<Recipe> _allRecipes;

        [Tooltip("Liste des ingrédients actuellement dans le chaudron.")]
        [SerializeField] private List<ItemData> _currentIngredients = new List<ItemData>();

        [Tooltip("Référence à l'objet Scriptable contenant les potions que le player a.")]
        [SerializeField] private PotionListData _potionListData;

        private CauldronTimer _cauldronTimer;
        private CauldronMixing _cauldronMixing;
        private CauldronVFXController _cauldronVFXController;


        private bool _isActive = false;
        private bool _qteSuccess;
        private bool _qteInProgress = false;
        private Recipe _currentRecipe;
        private int _currentRecipeIndex;
        private bool _needItem = true;
        private ItemData _ingredientToAdd;

        public bool QteInProgress
        {
            get => _qteInProgress;
            set => _qteInProgress = value;
        }

        public bool NeedItem
        {
            get => _needItem;
            set => _needItem = value;
        }

        private void Awake()
        {
            _cauldronMixing = GetComponent<CauldronMixing>();
            _cauldronTimer = GetComponent<CauldronTimer>();
            _cauldronVFXController = GetComponent<CauldronVFXController>();

            if (_cauldronTimer == null)
            {
                Debug.LogError("CauldronTimer n'est pas attaché au chaudron !");
            }
            if (_cauldronVFXController == null)
            {
                Debug.LogError("CauldronVFXController n'est pas attaché au chaudron !");
            }
        }

        public void ToggleShowInteractivity()
        {
            _isActive = !_isActive;
        }

        public void AddIngredient(ItemData ingredient)
        {
            if (ingredient == null)
                return;

            if (!_currentIngredients.Any())
            {
                InitializeRecipeWithFirstIngredient(ingredient);
            }

            if (_currentRecipe == null || ingredient != _currentRecipe.RequiredIngredients[_currentRecipeIndex])
            {
                HandleFailedPotion();
                return;
            }

            _ingredientToAdd = ingredient;
            _needItem = false;
            _cauldronVFXController.PlayBubble();

            if (ingredient.CanBeStirred)
            {
                StartQTEForStirring(ingredient);
            }
            else
            {
                StartCoroutine(HandleIngredientWithoutStir(ingredient));
            }
        }

        private void InitializeRecipeWithFirstIngredient(ItemData ingredient)
        {
            foreach (Recipe recipe in _allRecipes)
            {
                if (recipe.RequiredIngredients[0] == ingredient)
                {
                    _currentRecipe = recipe;
                    _currentRecipeIndex = 0;
                    break;
                }
            }

            if (_currentRecipe == null)
            {
                Debug.Log("Recipe is null");
                _cauldronVFXController.PlayBurned();
            }
        }

        private void StartQTEForStirring(ItemData ingredient)
        {
            QteInProgress = true;

            OnStirStarted?.Invoke();
            
            PlayerInteraction playerInteraction = FindFirstObjectByType<PlayerInteraction>();
            PlayerInput input = playerInteraction.GetComponent<PlayerInput>();

            input?.SwitchCurrentActionMap("QTE");

            _cauldronMixing.ConvertItem(playerInteraction);
        }

        private IEnumerator HandleIngredientWithoutStir(ItemData ingredient)
        {
            _cauldronTimer.PotionSuccessExpected = true;
            Recipe localRecipe = _currentRecipe;

            _cauldronTimer.StartCooldown();
            yield return new WaitForSeconds(_cauldronTimer.RemainingTime);

            if (_currentRecipe != localRecipe)
            {
                Debug.LogWarning("Recette modifiée pendant le cooldown.");
                yield break;
            }

            ValidateIngredientWithoutQTE(ingredient);
        }

        private void ValidateIngredientWithoutQTE(ItemData ingredient)
        {
            if (_currentRecipe == null)
            {
                Debug.LogWarning("ValidateIngredientAddition appelé sans recette active !");
                HandleFailedPotion();
                return;
            }

            if (_currentRecipe.RequiredIngredients[_currentRecipeIndex] == ingredient)
            {
                _currentIngredients.Add(ingredient);
                _currentRecipeIndex++;
                _cauldronTimer.ResetCooldown();
                _cauldronTimer.TimerIsActive = true;

                CheckRecipeCompletion();
            }
            else
            {
                HandleFailedPotion();
            }
        }

        private void ValidateIngredientAddition(ItemData ingredient)
        {
            if (_currentRecipe == null)
            {
                Debug.LogWarning("ValidateIngredientAddition appelé sans recette active !");
                HandleFailedPotion();
                return;
            }

            if (!_qteSuccess)
            {
                HandleFailedPotion();
                return;
            }

            if (_currentRecipe.RequiredIngredients[_currentRecipeIndex] == ingredient)
            {
                _currentIngredients.Add(ingredient);
                _currentRecipeIndex++;
                _cauldronTimer.ResetCooldown();
                _cauldronTimer.TimerIsActive = true;

                CheckRecipeCompletion();
            }
            else
            {
                HandleFailedPotion();
            }
        }

        private void CheckRecipeCompletion()
        {
            if (IsRecipeComplete(_currentRecipe))
            {
                HandleSuccessfulPotion(_currentRecipe);
            }
            else
            {
                _needItem = true;
            }
        }

        private bool IsRecipeComplete(Recipe recipe)
        {
            return recipe.RequiredIngredients.Count == _currentRecipeIndex;
        }

        private void HandleSuccessfulPotion(Recipe recipe)
        {
            _needItem = true;
            _currentRecipe = null;
            _cauldronTimer.StopCooldown();
            _potionListData.PotionResults.Add(recipe.Potion);
            _currentIngredients.Clear();
            _ingredientToAdd = null;
        }

        private void HandleFailedPotion()
        {
            _cauldronVFXController.PlayBurned();
            _needItem = true;
            _currentRecipe = null;
            _cauldronTimer.StopCooldown();
            _cauldronMixing.DesactiveQTE();
            _currentIngredients.Clear();
            _ingredientToAdd = null;
        }

        public void CheckQTE(bool state)
        {
            QteInProgress = false;
            _qteSuccess = state;

            OnStirEnded?.Invoke();

            if (state)
            {
                ValidateIngredientAddition(_ingredientToAdd);
            }
            else
            {
                HandleFailedPotion();
            }
        }
    }
}