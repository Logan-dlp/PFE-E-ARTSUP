using MoonlitMixes.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Inventory
{
    public class ToggleInventory : MonoBehaviour
    {
        [SerializeField] private GameObject _canvaInventory;
        [SerializeField] private GameObject _canvaChestInventory;
        [SerializeField] private GameObject _canvaChestFullText;
        [SerializeField] private GameObject _content;
        private InventoryUI _inventoryUI;
        private DiscardInventory _discardInventory;
        private InputManager _inputManager;
        private bool _isActive = false;

        private void Start()
        {
            _inventoryUI = gameObject.GetComponent<DiscardInventory>().InventoryUI;
            _inputManager = FindFirstObjectByType<InputManager>();
            _discardInventory= gameObject.GetComponent<DiscardInventory>();
            if (_canvaInventory != null)
            {
                _canvaInventory.SetActive(_isActive);
            }
        }
        public void Toggle(bool state)
        {
            if (state)
            {
                _inputManager.SwitchActionMap("UI");
                StartCoroutine(_inventoryUI.SelectedButton());
            }
            else if (_canvaInventory.activeInHierarchy || _canvaChestInventory.activeInHierarchy) _inputManager.SwitchActionMap("PlayerMovement");
            _canvaInventory.SetActive(state);
            _canvaChestInventory.SetActive(false);
            _canvaChestFullText.SetActive(false);
        }
        public void Discard(InputAction.CallbackContext context)
        {
            if(context.canceled)
            {
                if (_canvaInventory.activeInHierarchy) _discardInventory.DiscardBag();
                else _discardInventory.DiscardChest();
            }
        }
    }
}