using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.UI
{
    public class TurnPageCredits : MonoBehaviour
    {
        [SerializeField] private ScriptableCallbackContextEvent _scriptableCallbackContextEvent;
        [SerializeField] private GameObject[] _creditsArray; 

        private int _creditIndex;

        private void OnEnable()
        {
            _scriptableCallbackContextEvent.OnContextEvent += ChangeCredit;
        }

        private void OnDisable()
        {
            _scriptableCallbackContextEvent.OnContextEvent -= ChangeCredit;
        }

        public void ChangeCredit(InputAction.CallbackContext context)
        {
            if (context.started && gameObject.activeInHierarchy)
            {
                if (context.ReadValue<Vector2>().x > 0.5)
                {
                    if (_creditIndex < _creditsArray.Length - 1)
                    {
                        _creditIndex++;
                    }
                    else
                    {
                        _creditIndex = 0;
                    }
                }
                else if (context.ReadValue<Vector2>().x < -0.5)
                {
                    if (_creditIndex > 0)
                    {
                        _creditIndex--;
                    }
                    else
                    {
                        _creditIndex = _creditsArray.Length - 1;
                    }
                }

                foreach (GameObject credit in _creditsArray)
                {
                    credit.SetActive(false);
                }

                _creditsArray[_creditIndex].SetActive(true);
            }
        }
    }
}
