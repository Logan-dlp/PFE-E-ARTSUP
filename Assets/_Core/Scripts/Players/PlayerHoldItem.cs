using MoonlitMixes.Player;
using UnityEngine;

public class PlayerHoldItem : MonoBehaviour
{
    [SerializeField] private GameObject _itemHoldPivot;
    [SerializeField] private ScriptableItemEvent _scriptableItemEvent;
    private ItemData _item;
    private GameObject _itemHold;

    public ItemData Item
    {
        get => _item;
        set => _item = value;
    } 
    public GameObject ItemHold
    {
        get => _itemHold;
        set => _itemHold = value;
    }

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
        _itemHold.transform.localPosition = Vector3.zero;
        _itemHold.transform.localScale = Vector3.one;
    }
    
    public void GetItemData(GameObject item)
    {
        _itemHold = Instantiate(item, _itemHoldPivot.transform);
        _item = _itemHold.GetComponent<ItemDataHolder>().ItemData;
        DisplayItemHold();
        GetComponent<PlayerInteraction>().ItemInHand = _itemHold;
    }
}
