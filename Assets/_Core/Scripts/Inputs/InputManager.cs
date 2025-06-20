using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Inputs
{
    using Singleton;

    public class InputManager : MonoSingleton<InputManager>
    {
        private PlayerInput _currentPlayerInput;

        protected override void Awake()
        {
            base.Awake();
            _currentPlayerInput = FindFirstObjectByType<PlayerInput>();
        }

        public void SwitchActionMap(string mappingName)
        {
            if (_currentPlayerInput.actions.FindActionMap(mappingName) != null)
            {
                _currentPlayerInput.SwitchCurrentActionMap(mappingName);
            }
            else
            {
                Debug.LogError($"No mapping found for {mappingName}.");
            }
        }
    }
}