using MoonlitMixes.Extensions;
using MoonlitMixes.Item;
using MoonlitMixes.Potion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    public class ChangeRecipeUI : MonoBehaviour
    {
        [SerializeField] private GameObject _headerRecipe;
        [SerializeField] private GameObject _recipeTemplate;
        [SerializeField] private GameObject _recipeParent;
        [SerializeField] private ScriptableCallbackContextEvent _scriptableCallbackContextEvent;
        [SerializeField] private Recipe[] _recipeArray;
        [SerializeField] private Sprite[] _numberSpriteArray;
        [SerializeField] private Sprite[] _actionSpriteArray;
        [SerializeField] private RecipeTemplate recipeTemplate;

        private int _recipeIndex;

        private void OnEnable()
        {
            _scriptableCallbackContextEvent.OnContextEvent += ChangeRecipe;
        }

        private void OnDisable()
        {
            _scriptableCallbackContextEvent.OnContextEvent -= ChangeRecipe;
        }

        public void ChangeRecipe(InputAction.CallbackContext context)
        {
            if (context.started && _recipeParent.activeInHierarchy)
            {
                if (context.ReadValue<Vector2>().x > 0.5)
                {
                    if (_recipeIndex < _recipeArray.Length - 1)
                    {
                        _recipeIndex++;
                    }
                    else
                    {
                        _recipeIndex = 0;
                    }
                }
                else if (context.ReadValue<Vector2>().x < -0.5)
                {
                    if (_recipeIndex > 0)
                    {
                        _recipeIndex--;
                    }
                    else
                    {
                        _recipeIndex = _recipeArray.Length - 1;
                    }
                }

                if (_recipeIndex == 0)
                {
                    Debug.Log("");
                    _headerRecipe.SetActive(true);
                    _recipeTemplate.SetActive(false);
                }
                else
                {
                    _headerRecipe.SetActive(false);
                    _recipeTemplate.SetActive(true);
                    SetTempalateInfo(_recipeArray[_recipeIndex]);
                }
            }
        }

        private void SetTempalateInfo(Recipe recipe)
        {
            GameObject item;
            GameObject number;
            GameObject action;
            
            if (_recipeIndex % 2 == 0)
            {
                recipeTemplate.notePotion1.SetActive(false);
                recipeTemplate.notePotion2.SetActive(true);
                recipeTemplate.noteName2.text = recipe.RecipeName;
                recipeTemplate.noteDescription2.text = recipe.Description;
            }
            else
            {
                recipeTemplate.notePotion1.SetActive(true);
                recipeTemplate.notePotion2.SetActive(false);
                recipeTemplate.noteName1.text = recipe.RecipeName;
                recipeTemplate.noteDescription1.text = recipe.Description;
            }

            recipeTemplate.potionImage.sprite = recipe.PotionSprite;

            recipeTemplate.frameUI.transform.DestroyAllChild();
            recipeTemplate.itemUI.transform.DestroyAllChild();
            recipeTemplate.numberUI.transform.DestroyAllChild();
            recipeTemplate.actionUI.transform.DestroyAllChild();

            for (int i = 0; i < recipe.RequiredIngredients.Count; i++)
            {
                Instantiate(recipeTemplate.framePrefab, recipeTemplate.frameUI.transform);
                item = Instantiate(recipeTemplate.itemPrefab, recipeTemplate.itemUI.transform);
                number = Instantiate(recipeTemplate.numberPrefab, recipeTemplate.numberUI.transform);
                action = Instantiate(recipeTemplate.actionPrefab, recipeTemplate.actionUI.transform);

                number.GetComponent<Image>().sprite = _numberSpriteArray[i];

                if (recipe.RequiredIngredients[i].State == ItemUsage.Whole)
                {
                    item.GetComponent<Image>().sprite = recipe.RequiredIngredients[i].ItemSprite;
                }
                else
                {
                    item.GetComponent<Image>().sprite = recipe.RequiredIngredients[i].SpriteItemOrigin;
                }

                switch (recipe.RequiredIngredients[i].Usage)
                {
                    case ItemUsage.Whole:
                        action.GetComponent<Image>().sprite = _actionSpriteArray[0];
                        break;
                    case ItemUsage.Crush:
                        action.GetComponent<Image>().sprite = _actionSpriteArray[1];
                        break;
                    case ItemUsage.Cut:
                        action.GetComponent<Image>().sprite = _actionSpriteArray[2];
                        break;
                    default:
                        action.GetComponent<Image>().sprite = _actionSpriteArray[3];
                        break;
                }
            }
        }
    }
}