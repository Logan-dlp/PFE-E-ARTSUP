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
        [SerializeField] private List<GameObject> _itemsList;

        public void OpenChest()
        {
            _inventoryChestUI.RefreshInventory();
            GameObject B = _chestUI.transform.GetChild(0).GetChild(0).gameObject;
            EventSystem.current.SetSelectedGameObject(B);
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
        }
        private bool EnoughPlaceInChest()
        {
            if (_inventoryChestUI.EmptySlot> _inventoryUI.Items.Count- _inventoryUI.EmptySlot) { return true; }
            else { return false; }

        }
        [Button]
        public void ResetList()
        {
            _chestInventory.Items.Clear();
        }
        private void UpdateChest()
        {
            for (int i = 0; i < _itemsList.Count; i++)
            { 
             
            }
        }
    }
}