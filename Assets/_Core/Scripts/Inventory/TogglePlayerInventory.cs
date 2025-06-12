using MoonlitMixes.Inputs;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

namespace MoonlitMixes.Inventory
{
    public class ToggleInventory : MonoBehaviour
    {
        [SerializeField] private GameObject _canvaInventory;
        [SerializeField] private GameObject _canvaChestInventory;
        [SerializeField] private GameObject _content;
        private DiscardInventory _discardInventory;
        private InputManager _inputManager;
    
        private bool _isActive = false;

        private void Start()
        {
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
                if (_content.transform.childCount > 0)
                {
                    Debug.Log("nb child = " + _content.transform.childCount);
                    GameObject B = _content.transform.GetChild(0).gameObject;
                    EventSystem.current.SetSelectedGameObject(B);
                    EventSystem.current.firstSelectedGameObject = B;
                }
            }
            else if (_canvaInventory.activeInHierarchy || _canvaChestInventory.activeInHierarchy) _inputManager.SwitchActionMap("Player");
            _canvaInventory.SetActive(state);
            _canvaChestInventory.SetActive(false);
        }
        public void Discard()
        {
            if (_canvaInventory.activeInHierarchy) _discardInventory.DiscardBag();
            else _discardInventory.DiscardChest();
        }
    }
}