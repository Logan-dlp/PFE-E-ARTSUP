using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace MoonlitMixes.AI.PNJ
{
    public class CloseOrOpenShop : MonoBehaviour
    {
        [SerializeField] private GameObject _openText;
        [SerializeField] private GameObject _closeText;
        [SerializeField] private CustomerSpawner _customerSpawner;

        private bool _isPlayerInTrigger = false;
        private bool _isShopOpen = false;

        public static event Action<bool> OnShopToggled;

        private void Start()
        {
            _isShopOpen = false;
            UpdateUI();
            _customerSpawner.OnAllCustomersGone += AutoCloseShop;
        }

        private void OnDestroy()
        {
            _customerSpawner.OnAllCustomersGone -= AutoCloseShop;
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
            if (context.performed && _isPlayerInTrigger && !_isShopOpen)
            {
                _isShopOpen = true;
                UpdateUI();
                OnShopToggled?.Invoke(true);
                _customerSpawner.StartSpawning();
            }
        }

        public void AutoCloseShop()
        {
            _isShopOpen = false;
            UpdateUI();
            OnShopToggled?.Invoke(false);
        }

        private void UpdateUI()
        {
            _openText.SetActive(_isShopOpen);
            _closeText.SetActive(!_isShopOpen);
        }
    }
}