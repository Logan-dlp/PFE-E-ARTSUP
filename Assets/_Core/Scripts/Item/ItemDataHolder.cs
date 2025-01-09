using UnityEngine;

namespace MoonlitMixes.Item
{
    public class ItemDataHolder : MonoBehaviour
    {
        [SerializeField] private ItemData _itemData;

        public ItemData ItemData { get; set; }
    }
}
