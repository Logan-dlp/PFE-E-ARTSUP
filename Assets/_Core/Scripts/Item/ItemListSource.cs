using UnityEngine;
using MoonlitMixes.Item;

namespace MoonlitMixes.Item
{
    public class ItemListSource : MonoBehaviour
    {
        [SerializeField] private ItemListData _itemListData;

        public ItemListData GetItemList()
        {
            return _itemListData;
        }
    }
}