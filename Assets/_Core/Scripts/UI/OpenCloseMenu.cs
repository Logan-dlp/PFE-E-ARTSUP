using MoonlitMixes.Events;
using MoonlitMixes.Inputs;
using UnityEngine;

namespace MoonlitMixes.UI
{
    public class OpenCloseMenu : MonoBehaviour
    {
        [SerializeField] private ScriptableBoolEvent _scriptableBoolEvent;
        [SerializeField] private GameObject _menuUI;

        private void OnEnable()
        {
            _scriptableBoolEvent.BoolAction += OpenClose;
        }
        private void OnDisable()
        {
            _scriptableBoolEvent.BoolAction -= OpenClose;
        }


        private void OpenClose(bool state)
        {
            _menuUI.SetActive(state);
            if(state) InputManager.Instance.SwitchActionMap("Menu");
            else InputManager.Instance.SwitchActionMap("PlayerMovement");
        }
    }
}
