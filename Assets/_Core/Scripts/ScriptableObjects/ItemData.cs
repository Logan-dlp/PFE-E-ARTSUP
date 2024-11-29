using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _objectName;
    [SerializeField] private ElementType _elementType;
    [SerializeField, Range(1, 4)] private int _rarity;
    [SerializeField] private ItemUsage _itemUsage;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private ItemData _itemToTransform;
    [SerializeField] private string _description;

    public string ObjectName
    {
        get => _objectName;
        set => _objectName = value;
    }

    public ElementType Type
    {
        get => _elementType;
        set => _elementType = value;
    }

    public int Rarity
    {
        get => _rarity;
        set => _rarity = value;
    }

    public ItemUsage Usage
    {
        get => _itemUsage;
        set => _itemUsage = value;
    }

    public Sprite ItemSprite
    {
        get => _sprite;
        set => _sprite = value;
    }

    public ItemData ItemToTransform
    {
        get => _itemToTransform;
        set => _itemToTransform = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public ItemData TransformToOtherItem()
    {
        return _itemToTransform;
    }
}