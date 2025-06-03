using MoonlitMixes.Extensions;
using MoonlitMixes.Item;
using MoonlitMixes.Potion;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    public class ChangeRecipeUI : MonoBehaviour
    {
        [SerializeField] private GameObject _headerRecipe;
        [SerializeField] private GameObject _recipeTemplateUI;
        [SerializeField] private GameObject _recipeParent;
        [SerializeField] private ScriptableCallbackContextEvent _scriptableCallbackContextEvent;
        [SerializeField] private Recipe[] _recipeArray;
        [SerializeField] private Sprite[] _numberSpriteArray;
        [SerializeField] private Sprite[] _actionSpriteArray;
        [SerializeField] private Sprite[] _potionSpriteArray;
        [SerializeField] private RecipeTemplate _recipeTemplate;
        [SerializeField] private ItemData _insect;
        [SerializeField] private TMP_Text _pageIndex;

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
                    _recipeTemplateUI.SetActive(false);
                }
                else
                {
                    _headerRecipe.SetActive(false);
                    _recipeTemplateUI.SetActive(true);
                    SetTempalateInfo(_recipeArray[_recipeIndex]);
                }
                _pageIndex.text = (_recipeIndex + 1).ToString();
            }
        }

        private void SetTempalateInfo(Recipe recipe)
        {
            GameObject item;
            GameObject number;
            GameObject action;

            if (_recipeIndex % 2 == 0)
            {
                _recipeTemplate.notePotion1.SetActive(false);
                _recipeTemplate.notePotion2.SetActive(true);
                _recipeTemplate.noteName2.text = recipe.RecipeName;
                _recipeTemplate.noteDescription2.text = recipe.Description;
            }
            else
            {
                _recipeTemplate.notePotion1.SetActive(true);
                _recipeTemplate.notePotion2.SetActive(false);
                _recipeTemplate.noteName1.text = recipe.RecipeName;
                _recipeTemplate.noteDescription1.text = recipe.Description;
            }

            _recipeTemplate.potionImage.sprite = _potionSpriteArray[_recipeIndex];
            _recipeTemplate.potionImage.preserveAspect = true;

            _recipeTemplate.frameUI.transform.DestroyAllChild();
            _recipeTemplate.itemUI.transform.DestroyAllChild();
            _recipeTemplate.numberUI.transform.DestroyAllChild();
            _recipeTemplate.actionUI.transform.DestroyAllChild();

            for (int i = 0; i < recipe.RequiredIngredients.Count; i++)
            {
                Instantiate(_recipeTemplate.framePrefab, _recipeTemplate.frameUI.transform);
                item = Instantiate(_recipeTemplate.itemPrefab, _recipeTemplate.itemUI.transform);
                number = Instantiate(_recipeTemplate.numberPrefab, _recipeTemplate.numberUI.transform);
                action = Instantiate(_recipeTemplate.actionPrefab, _recipeTemplate.actionUI.transform);

                number.GetComponent<Image>().sprite = _numberSpriteArray[i];
                number.GetComponent<Image>().preserveAspect = true;

                if (recipe.RequiredIngredients[i].State == ItemUsage.Whole)
                {
                    item.GetComponent<Image>().sprite = recipe.RequiredIngredients[i].ItemSprite;
                    item.GetComponent<Image>().preserveAspect = true;
                }
                else
                {
                    item.GetComponent<Image>().sprite = recipe.RequiredIngredients[i].SpriteItemOrigin;
                    item.GetComponent<Image>().preserveAspect = true;
                }

                if (recipe.RequiredIngredients[i].State == ItemUsage.Whole)
                {
                    switch (recipe.RequiredIngredients[i].Usage)
                    {
                        case ItemUsage.Whole:
                            action.GetComponent<Image>().sprite = _actionSpriteArray[0];
                            action.GetComponent<Image>().preserveAspect = true;
                            break;
                        case ItemUsage.Crush:
                            action.GetComponent<Image>().sprite = _actionSpriteArray[1];
                            action.GetComponent<Image>().preserveAspect = true;
                            break;
                        case ItemUsage.Cut:
                            action.GetComponent<Image>().sprite = _actionSpriteArray[2];
                            action.GetComponent<Image>().preserveAspect = true;
                            break;
                        default:
                            action.GetComponent<Image>().sprite = _actionSpriteArray[3];
                            action.GetComponent<Image>().preserveAspect = true;
                            break;
                    }
                }
                else if (recipe.RequiredIngredients[i] == _insect)
                {
                    action.GetComponent<Image>().sprite = _actionSpriteArray[4];
                    action.GetComponent<Image>().preserveAspect = true;
                }
                else
                {
                    switch (recipe.RequiredIngredients[i].State)
                    {
                        case ItemUsage.Whole:
                            action.GetComponent<Image>().sprite = _actionSpriteArray[0];
                            action.GetComponent<Image>().preserveAspect = true;
                            break;
                        case ItemUsage.Crush:
                            action.GetComponent<Image>().sprite = _actionSpriteArray[1];
                            action.GetComponent<Image>().preserveAspect = true;
                            break;
                        case ItemUsage.Cut:
                            action.GetComponent<Image>().sprite = _actionSpriteArray[2];
                            action.GetComponent<Image>().preserveAspect = true;
                            break;
                        default:
                            action.GetComponent<Image>().sprite = _actionSpriteArray[3];
                            action.GetComponent<Image>().preserveAspect = true;
                            break;
                    }
                }
            }
        }
    }
}