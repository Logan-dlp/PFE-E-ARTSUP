using System.Collections;
using System.Collections.Generic;
using MoonlitMixes.Datas;
using MoonlitMixes.Inputs;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitMixes.Inventory
{
    public class ChestInteraction : MonoBehaviour
    {
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
            GameObject B = _chestUI.transform.GetChild(0).gameObject;
            EventSystem.current.SetSelectedGameObject(B);
            InputManager.Instance.SwitchActionMap("UI");
            Debug.Log(EnoughPlaceInChest());
            Debug.Log(_inventoryChestUI.EmptySlot);
            Debug.Log(_inventoryUI.Items.Count - _inventoryUI.EmptySlot);
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
        }
        private bool EnoughPlaceInChest()
        {
            if (_inventoryChestUI.EmptySlot>= _inventoryUI.Items.Count- _inventoryUI.EmptySlot) { return true; }
            else { return false; }

        }
    }
}