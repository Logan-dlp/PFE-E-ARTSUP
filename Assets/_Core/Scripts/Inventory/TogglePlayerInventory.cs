using MoonlitMixes.Inputs;
using UnityEngine;

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

        public void Toggle(bool state)
        {
            
            if(state) _inputManager.SwitchActionMap("UI");
            else if(_canvaInventory.activeInHierarchy) _inputManager.SwitchActionMap("Player");
            
            _canvaInventory.SetActive(state);
        }
    }
}