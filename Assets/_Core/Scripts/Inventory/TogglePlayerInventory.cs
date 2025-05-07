using MoonlitMixes.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Inventory
{
    public class ToggleInventory : MonoBehaviour
    {
        [SerializeField] private GameObject _canvaInventory;
        private InputManager _inputManager;
    
        private bool isActive = false;

        private void Start()
        {
            _inputManager = FindFirstObjectByType<InputManager>();
            if (_canvaInventory != null)
            {
                _canvaInventory.SetActive(isActive);
            }
        }

        public void Toggle(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                isActive = !isActive;
                _canvaInventory.SetActive(isActive);

                if(isActive) _inputManager.SwitchActionMap("UI");
                else _inputManager.SwitchActionMap("Player");
            }
        }
    }
}