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
        private bool _cooldownCoroutineRunning = false;
        private Coroutine _currentCooldownCoroutine;


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

            // Si on est dans la phase attente, on relance le timer (phase validation)
            if (_cauldronTimer.WaitingForNextIngredient)
            {
                _cauldronTimer.WaitingForNextIngredient = false;
                _cauldronTimer.ResetCooldown();
                _cauldronTimer.TimerIsActive = true;
            }
            else if (_currentCooldownCoroutine != null)
            {
                StopCoroutine(_currentCooldownCoroutine);
                Debug.Log("Ancien timer interrompu.");
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
                _cauldronTimer.PotionSuccessExpected = true;
                _cauldronTimer.StartCooldown();
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
            _cooldownCoroutineRunning = true;

            _cauldronTimer.PotionSuccessExpected = true;
            Recipe localRecipe = _currentRecipe;

            _cauldronTimer.StartCooldown();
            float duration = _cauldronTimer.RemainingTime;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                yield return null;
                elapsed += Time.deltaTime;
            }

            ValidateIngredientWithoutQTE(ingredient);
            _cooldownCoroutineRunning = false;
            _currentCooldownCoroutine = null;

        }

        private void ValidateIngredientWithoutQTE(ItemData ingredient)
        {
            Debug.Log($"VALIDATE WITHOUT QTE: Recipe = {_currentRecipe?.name}, Index = {_currentRecipeIndex}, Ingredient = {ingredient.name}, Expected = {_currentRecipe?.RequiredIngredients[_currentRecipeIndex].name}");

            Debug.Log("Validate sans QTE");
            _cooldownCoroutineRunning = false;
            _currentCooldownCoroutine = null;

            if (_currentRecipe == null)
            {
                Debug.LogWarning("ValidateIngredientAddition appelé sans recette active !");
                HandleFailedPotion();
                return;
            }

            if (_currentRecipe.RequiredIngredients[_currentRecipeIndex] == ingredient)
            {
                Debug.Log("ingédient good");
                _currentIngredients.Add(ingredient);
                _currentRecipeIndex++;

                CheckRecipeCompletion();
            }
            else
            {
                HandleFailedPotion();
            }
        }

        private void ValidateIngredientAddition(ItemData ingredient)
        {
            Debug.Log("Validate QTE");
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

        public void HandleFailedPotion()
        {
            Debug.LogWarning("💥 Potion échouée — état courant: " +
                 $"Recipe = {_currentRecipe?.name}, Index = {_currentRecipeIndex}, Ingredients = {_currentIngredients.Count}");

            _cooldownCoroutineRunning = false;

            _cauldronVFXController.PlayBurned();
            _needItem = true;
            _currentRecipe = null;
            _cauldronTimer.StopCooldown();
            _cauldronMixing.DesactiveQTE();
            _currentIngredients.Clear();
            _ingredientToAdd = null;
            _cauldronTimer.CanAction = true;
            _cauldronTimer.TimerIsActive = false;
            _cauldronTimer.PotionSuccessExpected = false;
        }

        public void ValidateIngredientAfterTimer()
        {
            if (_ingredientToAdd != null)
            {
                ValidateIngredientWithoutQTE(_ingredientToAdd);
            }
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