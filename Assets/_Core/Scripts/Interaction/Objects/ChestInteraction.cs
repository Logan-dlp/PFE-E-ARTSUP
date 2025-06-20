using MoonlitMixes.Datas;
using MoonlitMixes.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace MoonlitMixes.Inventory
{
    public class ChestInteraction : MonoBehaviour
    {
        public static Action OnChestOpened;

        [SerializeField] private GameObject _chestUI;
        [SerializeField] private GameObject _inventoryFullText;
        [SerializeField] private InventoryData _playerInventory;
        [SerializeField] private InventoryData _chestInventory;
        [SerializeField] private InventoryUI _inventoryUI;
        [SerializeField] private InventoryUI _inventoryChestUI;
        
        public void OpenChest()
        {
            _inventoryChestUI.RefreshInventory();
            _inventoryUI.RefreshInventory();
            StartCoroutine(_inventoryChestUI.SelectedButton());
            InputManager.Instance.SwitchActionMap("UI");
            if (EnoughPlaceInChest())
            {
                _inventoryFullText.SetActive(false);
                _inventoryUI.SendItems();
                _chestUI.SetActive(true);
            }
            else
            {
                _inventoryFullText.SetActive(true);
                _chestUI.SetActive(true);
            }

            OnChestOpened?.Invoke();
        }
        
        private bool EnoughPlaceInChest()
        {
            if (_inventoryChestUI.EmptySlot>= _inventoryUI.Items.Count- _inventoryUI.EmptySlot) { return true; }
            else { return false; }

        }
    }
}