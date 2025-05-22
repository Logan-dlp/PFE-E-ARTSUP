using MoonlitMixes.Potion;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.UI
{
    public class ChangeRecipeUI : MonoBehaviour
    {
        [SerializeField] private GameObject _headerRecipe;
        [SerializeField] private GameObject _recipeTemplate;
        [SerializeField] private Recipe[] _recipeArray;
        [SerializeField] private ScriptableCallbackContextEvent _scriptableCallbackContextEvent;
        public RecipeTemplate recipeTemplate;

        private int _recipeIndex;

        private void Start()
        {

        }

        private void OnEnable()
        {
            _scriptableCallbackContextEvent.OnContextEvent += ChangeRecipe;
        }

        private void OnDisable()
        {
            _scriptableCallbackContextEvent.OnContextEvent += ChangeRecipe;
        }

        public void ChangeRecipe(InputAction.CallbackContext context)
        {
            if (context.started && gameObject.activeInHierarchy)
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
                    _headerRecipe.SetActive(true);
                    _recipeTemplate.SetActive(false);
                }
                else
                {
                    SetTempalateInfo(_recipeArray[_recipeIndex]);
                }
            }
        }

        private void SetTempalateInfo(Recipe recipe)
        {

        }
    }
}
