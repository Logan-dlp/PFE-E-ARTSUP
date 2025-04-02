using UnityEngine;
using System.Collections.Generic;
using MoonlitMixes.Item;

namespace MoonlitMixes.Datas.Inventory
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
    public class InventoryData : ScriptableObject
    {
        [SerializeField] private InventoryMode _inventoryMode = InventoryMode.InventoryPlayer;
        [SerializeField] private List<ItemData> _items = new List<ItemData>();
        [SerializeField] private int _maxSlots = 20;

        public InventoryMode Mode
        {
            get => _inventoryMode;
            set => _inventoryMode = value;
        }

        public int MaxSlots
        {
            get
            {
                if (_inventoryMode == InventoryMode.InventoryPlayer)
                {
                    return _maxSlots;
                }
                else
                {
                    return int.MaxValue;
                }
            }
            set
            {
                if (_inventoryMode == InventoryMode.InventoryPlayer)
                {
                    _maxSlots = value;
                }
            }

        }

        public List<ItemData> Items
        {
            get => _items;
            set => _items = value;
        }
    }
}