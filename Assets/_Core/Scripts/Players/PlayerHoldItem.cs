using MoonlitMixes.Player;
using UnityEngine;

public class PlayerHoldItem : MonoBehaviour
{
    [SerializeField] private GameObject _itemHoldPivot;
    [SerializeField] private ScriptableItemEvent _scriptableItemEvent;

    public ItemData Item { get; set; }
    public GameObject ItemHold { get; set; }

    private void OnEnable()
    {
        _scriptableItemEvent.ItemDataAction += GetItemData;
    }

    private void OnDisable()
    {
        _scriptableItemEvent.ItemDataAction -= GetItemData;
    }

    private void DisplayItemHold()
    {
        ItemHold.transform.localPosition = Vector3.zero;
        ItemHold.transform.localScale = Vector3.one;
    }
    
    public void GetItemData(GameObject item)
    {
        ItemHold = Instantiate(item, _itemHoldPivot.transform);
        Item = ItemHold.GetComponent<ItemDataHolder>().ItemData;
        DisplayItemHold();
        GetComponent<PlayerInteraction>().ItemInHand = ItemHold;
    }
}
