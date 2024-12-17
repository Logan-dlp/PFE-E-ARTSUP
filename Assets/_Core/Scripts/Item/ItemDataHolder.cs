using UnityEngine;

public class ItemDataHolder : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;

    public ItemData ItemData
    {
        get => _itemData;
        set => _itemData = value;
    }
}
