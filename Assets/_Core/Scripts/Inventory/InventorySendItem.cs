using MoonlitMixes.Datas;
using MoonlitMixes.Events;
using MoonlitMixes.Item;
using UnityEngine;

namespace MoonlitMixes.Inventory
{
    public class InventorySendItem : MonoBehaviour
    {
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private ScriptableItemEvent _scriptableItemEvent;

        public void SendItem()
        {
            ItemData itemToSend = GetComponentInChildren<ItemDataHolder>().ItemData;
            _scriptableItemEvent.SendObject(itemToSend.ItemPrefab);
            _inventory.Items.Remove(itemToSend);
        }
    }
}