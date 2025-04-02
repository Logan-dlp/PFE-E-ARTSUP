using MoonlitMixes.AI.PNJ;
using MoonlitMixes.Player.Interaction;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Shop
{
    public class CloseOrOpenShop : MonoBehaviour
    {
        public static event Action<bool> OnShopToggled;
        public bool HasShopBeenOpened => _hasShopBeenOpened;

        [SerializeField] private CustomerSpawner _customerSpawner;
        [SerializeField] private InteractionButton _interactionButton;

        private bool _isPlayerInTrigger = false;
        private bool _isShopOpen = false;
        private bool _hasShopBeenOpened = false;

        private void Start()
        {
            _isShopOpen = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerInTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _isPlayerInTrigger = false;
            }
        }

        public void OnToggleShop(InputAction.CallbackContext context)
        {
            if (_hasShopBeenOpened) return;

            if (context.performed && _isPlayerInTrigger)
            {
                if (!_isShopOpen)
                {
                    _isShopOpen = true;
                    _hasShopBeenOpened = true;
                    OnShopToggled?.Invoke(true);
                    _customerSpawner.StartSpawning();
                    _interactionButton.DeactivateButtonUI();
                }
            }
        }
    }
}