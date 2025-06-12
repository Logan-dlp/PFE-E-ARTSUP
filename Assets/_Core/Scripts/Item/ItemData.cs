using System;
using System.Numerics;
using MoonlitMixes.Extensions;
using MoonlitMixes.SaveSystems;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace MoonlitMixes.Item
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
    public class ItemData : ScriptableObject, ISerializable
    {
        [SerializeField] private string _obejctName;
        [SerializeField] private ElementType _elementType;
        [SerializeField, Range(1,4)] private int _rarity;
        [SerializeField] private ItemUsage _itemUsage;
        [SerializeField] private bool _canBeStirred;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private ItemData _itemToConvert;
        [SerializeField] private string _description;
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private ItemUsage _state;
        [SerializeField] private bool _isTransformed;

        public bool IsTransformed => _isTransformed;

        public bool CanBeStirred
        {
            get => _canBeStirred;
            set => _canBeStirred = value;
        }

        public string ObjectName
        {
            get => _obejctName;
        }

        public ElementType Type
        {
            get => _elementType;
        }

        public int Rarity
        {
            get => _rarity;
        }

        public ItemUsage Usage
        {
            get => _itemUsage;
        }

        public Sprite ItemSprite
        {
            get => _sprite;
        }

        public ItemData ItemToConvert
        {
            get => _itemToConvert;
        }

        public string Description
        {
            get => _description;
        }
        
        public GameObject ItemPrefab
        {
            get => _itemPrefab;
        }
        
        private struct SerializeData
        {
            public string obejctName;
            public ElementType elementType;
            public int rarity;
            public ItemUsage itemUsage;
            public string sprite;
            public string itemToConvert;
            public string description;
            public GameObject itemPrefab;
            public ItemUsage state;
        }

        public string Serialize()
        {
            SerializeData serializeData = new();

            serializeData.obejctName = _obejctName;
            serializeData.elementType = _elementType;
            serializeData.rarity = _rarity;
            serializeData.itemUsage = _itemUsage;
            serializeData.sprite = _sprite.Serialize();
            serializeData.itemToConvert = _itemToConvert != null ? _itemToConvert.ItemToConvert.Serialize() : null;
            serializeData.description = _description;
            serializeData.itemPrefab = _itemPrefab != null ? _itemPrefab : null;
            serializeData.state = _state;
            
            return SaveSystem.Instance.Serialize(serializeData);
        }

        public void Deserialize(string data)
        {
            SerializeData serializeData = SaveSystem.Instance.Deserialize<SerializeData>(data);
            
            _obejctName = serializeData.obejctName;
            _elementType = serializeData.elementType;
            _rarity = serializeData.rarity;
            _itemUsage = serializeData.itemUsage;
            _sprite.Deserialize(serializeData.sprite);
            _itemToConvert.Deserialize(serializeData.itemToConvert);
            _description = serializeData.description;
            _itemPrefab = serializeData.itemPrefab;
            _state = serializeData.state;
        }
    }
}