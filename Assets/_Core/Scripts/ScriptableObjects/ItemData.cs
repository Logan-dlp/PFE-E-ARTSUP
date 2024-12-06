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
    }

    public ElementType Type
    {
        get => _elementType;
    }

    public int Rarity
    {
        get => _rarity;
    }

    public ItemUsage Usage
    {
        get => _itemUsage;
    }

    public Sprite ItemSprite
    {
        get => _sprite;
    }

    public ItemData ItemToTransform
    {
        get => _itemToTransform;
    }

    public string Description
    {
        get => _description;
    }

    public ItemData TransformToOtherItem()
    {
        return _itemToTransform;
    }
}