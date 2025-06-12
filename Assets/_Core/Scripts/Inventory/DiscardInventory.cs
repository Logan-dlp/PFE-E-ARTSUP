using MoonlitMixes.Datas;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace MoonlitMixes.Inventory
{
    public class DiscardInventory : MonoBehaviour
    {
        [SerializeField] private InventoryData _playerInventory;
        [SerializeField] private InventoryData _chestInventory;
        [SerializeField] private InventoryUI _inventoryUI;
        [SerializeField] private InventoryUI _inventoryChestUI;

        
        public void DiscardBag()
        {
            string name = EventSystem.current.currentSelectedGameObject.name;
            Boolean result = char.IsDigit(name[name.Length - 1]);
            Boolean result2 = char.IsDigit(name[name.Length - 2]);
            if (result2)
            {

                int i = 0, y = 0;
                i = (int)char.GetNumericValue(name, name.Length - 1);
                y = (int)char.GetNumericValue(name, name.Length - 2);
                y = y * 10 + i;
                _playerInventory.Items[y] = _inventoryUI.EmptyData;
            }
            else
            {
                int i = 0;
                i = (int)char.GetNumericValue(name, name.Length - 1);
                _playerInventory.Items[i] = _inventoryUI.EmptyData;
            }
            _inventoryUI.RefreshInventory();

        }
        public void DiscardChest()
        {
            string name = EventSystem.current.currentSelectedGameObject.name;
            Boolean result = char.IsDigit(name[name.Length - 1]);
            Boolean result2 = char.IsDigit(name[name.Length - 2]);
            if (result2)
            {
                int i = 0, y = 0;
                i = (int)char.GetNumericValue(name, name.Length - 1);
                y = (int)char.GetNumericValue(name, name.Length - 2);
                y = y * 10 + i;
                _chestInventory.Items[y] = _inventoryChestUI.EmptyData;
            }
            else
            {
                int i = 0;
                i = (int)char.GetNumericValue(name, name.Length - 1);
                _chestInventory.Items[i] = _inventoryChestUI.EmptyData;
            }
            _inventoryChestUI.RefreshInventory();
        }
    }
}
