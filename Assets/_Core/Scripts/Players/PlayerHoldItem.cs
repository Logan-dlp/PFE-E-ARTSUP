using UnityEngine;

public class PlayerHoldItem : MonoBehaviour
{
    [SerializeField] private GameObject _itemHoldPivot;
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

    public void DisplayItemHold(ItemData itemData)
    {
        _item = itemData;
        _itemHold = Instantiate(_item.ItemPrefab, _itemHoldPivot.transform);
    }
}
