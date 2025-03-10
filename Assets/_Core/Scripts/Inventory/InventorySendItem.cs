using MoonlitMixes.Events;
using MoonlitMixes.Item;
using UnityEngine;

public class InventorySendItem : MonoBehaviour
{
    [SerializeField] private ScriptableItemEvent _scriptableItemEvent;

    public void SendItem()
    {
        _scriptableItemEvent.SendObject(GetComponentInChildren<ItemDataHolder>().ItemData.ItemPrefab);
    }
}
