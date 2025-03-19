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
        private Recipe _currentRecipe;
        private int _currentRecipeIndex;
        private bool _needMix;
        private bool _needItem = true;
        private ItemData _ingredentToAdd;
        
        public bool NeedMix
        {
            get => _needMix;
            set => _needMix = value;
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
            if(!_currentIngredients.Any())
            {
                _cauldronTimer.TimerIsActive = true;
                
                foreach (Recipe recipe in _allRecipes)
                {
                    if(recipe.RequiredIngredients[0] == ingredient)
                    {
                        _currentRecipe = recipe;
                        _currentRecipeIndex = 0;
                        break;
                    }
                }
            } 

            if (ingredient == null)
            {
                return;
            }
            
            if(ingredient != _currentRecipe.RequiredIngredients[_currentRecipeIndex]) 
            {
                HandleFailedPotion();
                return;
            }

            _ingredentToAdd = ingredient;
            _cauldronTimer.ResetCooldown();
            TriggerBubbleVFX();
            _needItem = false;
            _needMix = true;
        }

        private void ValidateIngredientAddition(ItemData ingredient)
        {
            if (!_qteSuccess)
            {
                HandleFailedPotion();
                return;
            }

            if(_currentRecipe.RequiredIngredients[_currentRecipeIndex] == ingredient)
            {
                _currentIngredients.Add(ingredient);
                _needMix = false;
                _currentRecipeIndex++;
                _cauldronTimer.ResetCooldown();
                CheckRecipeCompletion();
            }
        
        }

        private void CheckRecipeCompletion()
        {
            if (IsRecipeComplete(_currentRecipe))
            {
                HandleSuccessfulPotion(_currentRecipe);
                return;
            }
            else
            {
                _needItem = true;
            }
        }

        private bool IsRecipeComplete(Recipe recipe)
        {
            if(recipe.RequiredIngredients.Count == _currentRecipeIndex)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void HandleSuccessfulPotion(Recipe recipe)
        {
            _needItem = true;
            _currentRecipe = null;
            _cauldronTimer.StopCooldown();
            _potionInventory.PotionList.Add(recipe.Potion);
            _potionInventory.UpdatePotionCanvas();
            Debug.Log($"Recette r�ussie : {recipe.RecipeName} !");
            _currentIngredients.Clear();
        }

        private void HandleFailedPotion()
        {
            _needItem = true;
            _currentRecipe = null;
            _cauldronTimer.StopCooldown();
            Debug.Log("Potion rat�e !");
            _currentIngredients.Clear();
            _ingredentToAdd = null;
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
            ValidateIngredientAddition(_ingredentToAdd);
        }

        public void Mix(PlayerInteraction playerInteraction)
        {
            _cauldronMixing.ConvertItem(playerInteraction);
        }
    }
}