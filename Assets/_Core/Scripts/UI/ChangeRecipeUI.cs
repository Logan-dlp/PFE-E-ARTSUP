using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    public class ChangeRecipeUI : MonoBehaviour
    {
        [SerializeField] private Sprite[] _recipeArray;
        [SerializeField] private ScriptableCallbackContextEvent _scriptableCallbackContextEvent;

        private int _recipeIndex;
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
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

                _image.sprite = _recipeArray[_recipeIndex];
            }
        }
    }
}
