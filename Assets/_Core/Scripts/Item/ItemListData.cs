using System.Collections.Generic;
using MoonlitMixes.ExplorationTools;
using UnityEngine;

namespace MoonlitMixes.Item
{
    [CreateAssetMenu(fileName = "ItemList", menuName = "Scriptable Objects/Item List")]
    public class ItemListData : ScriptableObject
    {
        [SerializeField] private ToolType _toolType;
        [SerializeField] private List<ItemData> _items;

        public List<ItemData> Items
        {
            get => _items;
        }

        public ToolType ToolType
        {
            get => _toolType;
        }
    }
}