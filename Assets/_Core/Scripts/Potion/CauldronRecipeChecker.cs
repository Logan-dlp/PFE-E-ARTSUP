using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoonlitMixes.CookingMachine;
using MoonlitMixes.Datas;
using MoonlitMixes.Inputs;
using MoonlitMixes.Item;
using MoonlitMixes.Player;
using MoonlitMixes.Potion;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Potion
{
    public class CauldronRecipeChecker : MonoBehaviour
    {
        [SerializeField] private GameObject _interactUI;
        [SerializeField] private ParticleSystem _bubbleVFX;
        [SerializeField] private ParticleSystem _burnPot;
        [SerializeField] private List<Recipe> _allRecipes;
        [SerializeField] private List<ItemData> _currentIngredients = new List<ItemData>();
        [SerializeField] private PotionListData _potionListData;

        private CauldronTimer _cauldronTimer;
        private bool _isActive = false;
        private PotionInventory _potionInventory;
        private CauldronMixing _cauldronMixing;
        private bool _qteSuccess;
        public bool _qteInProgress = false;
        private Recipe _currentRecipe;
        private int _currentRecipeIndex;
        private bool _needItem = true;
        private ItemData _ingredentToAdd;

        public bool NeedItem
        {
            get => _needItem;
            set => _needItem = value;
        }

        private void Awake()
        {
            _cauldronMixing = GetComponent<CauldronMixing>();
            _cauldronTimer = GetComponent<CauldronTimer>();
            _potionInventory = FindFirstObjectByType<PotionInventory>();

            if (_cauldronTimer == null)
            {
                Debug.LogError("CauldronTimer n'est pas attaché au chaudron !");
            }
        }

        public void TogleShowInteractivity()
        {
            _isActive = !_isActive;
            //_interactUI.SetActive(_isActive);
        }

        public void AddIngredient(ItemData ingredient)
        {
            if (ingredient == null)
                return;

            if (!_currentIngredients.Any())
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
                    TriggerBurnPot();
                    return;
                }
            }

            if (_currentRecipe == null || ingredient != _currentRecipe.RequiredIngredients[_currentRecipeIndex])
            {
                HandleFailedPotion();
                return;
            }

            _ingredentToAdd = ingredient;
            TriggerBubbleVFX();
            _needItem = false;

            if (ingredient.CanBeStirred)
            {
                _qteInProgress = true;
                PlayerInteraction playerInteraction = FindFirstObjectByType<PlayerInteraction>();

                PlayerInput input = playerInteraction.GetComponent<PlayerInput>();
                if (input != null)
                {
                    input.SwitchCurrentActionMap("QTE");
                }

                _cauldronMixing.ConvertItem(playerInteraction);
            }
            else
            {
                StartCoroutine(HandleIngredientWithoutStir(ingredient));
            }
        }

        private IEnumerator HandleIngredientWithoutStir(ItemData ingredient)
        {
            _cauldronTimer.PotionSuccessExpected = true;

            Recipe localRecipe = _currentRecipe;

            _cauldronTimer.StartCooldown();
            Debug.Log("StartCooldown");

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
                Debug.Log("Success without QTE");

                CheckRecipeCompletion();
            }
            else
            {
                Debug.Log("Failed without QTE");
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
                Debug.Log("Failed QTE");
                HandleFailedPotion();
                return;
            }

            if (_currentRecipe.RequiredIngredients[_currentRecipeIndex] == ingredient)
            {
                _currentIngredients.Add(ingredient);
                _currentRecipeIndex++;

                _cauldronTimer.ResetCooldown();
                _cauldronTimer.TimerIsActive = true;
                Debug.Log("Success with QTE");

                CheckRecipeCompletion();
            }
            else
            {
                Debug.Log("Failed QTE logic");
                HandleFailedPotion();
            }
        }

        private void CheckRecipeCompletion()
        {
            if (IsRecipeComplete(_currentRecipe))
            {
                Debug.Log("Succes");
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
            _ingredentToAdd = null;
        }

        private void HandleFailedPotion()
        {
            _needItem = true;
            _currentRecipe = null;
            _cauldronTimer.StopCooldown();

            _cauldronMixing.DesactiveQTE();

            _currentIngredients.Clear();
            _ingredentToAdd = null;
        }

        private void TriggerBurnPot()
        {
            if (_burnPot != null)
            {
                _burnPot.Play();
                Invoke(nameof(DisableBurnPot), _burnPot.main.duration);
            }
        }

        private void DisableBurnPot()
        {
            if (_burnPot != null)
            {
                _burnPot.Stop();
            }
        }

        private void TriggerBubbleVFX()
        {
            if (_bubbleVFX != null)
            {
                _bubbleVFX.Play();
                Invoke(nameof(DisableBubbleVFX), _bubbleVFX.main.duration);
            }
        }

        private void DisableBubbleVFX()
        {
            if (_bubbleVFX != null)
            {
                _bubbleVFX.Stop();
            }
        }

        public void CheckQTE(bool state)
        {
            _qteInProgress = false;

            _qteSuccess = state;

            if (state)
            {
                ValidateIngredientAddition(_ingredentToAdd);
            }
            else
            {
                Debug.Log("Failed QTE");
                HandleFailedPotion();
            }
        }
    }
}