using UnityEngine;
using System.Collections.Generic;
using MoonlitMixes.Item;
using MoonlitMixes.SaveSystems;

namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
    public class InventoryData : ScriptableObject, ISerializable
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
        
        private struct SerializeData
        {
            public InventoryMode inventoryMode;
            public List<string> items;
            public int maxSlots;
        }

        public string Serialize()
        {
            SerializeData serializeData = new()
            {
                inventoryMode = _inventoryMode,
                maxSlots = _maxSlots,
            };

            foreach (ItemData itemData in _items)
            {
                serializeData.items.Add(itemData.Serialize());
            }

            return SaveSystem.Instance.Serialize(serializeData);
        }

        public void Deserialize(string data)
        {
            SerializeData serializeData = SaveSystem.Instance.Deserialize<SerializeData>(data);
            
            _inventoryMode = serializeData.inventoryMode;
            foreach (string serializeDataItem in serializeData.items)
            {
                ItemData newItem = new();
                newItem.Deserialize(serializeDataItem);
                _items.Add(newItem);
            }
            _maxSlots = serializeData.maxSlots;
        }
    }
}