using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _obejctName;
    [SerializeField] private ElementType _elementType;
    [SerializeField, Range(1,4)] private int _rarity;
    [SerializeField] private ItemUsage _itemUsage;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private ItemData _itemToConvert;
    [SerializeField] private string _description;
    [SerializeField] private GameObject _itemPrefab;
    
    public string ObjectName
    {
        get => _obejctName;
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
    
    public ItemData ItemToConvert
    {
        get => _itemToConvert;
    } 

    public string Description
    {
        get => _description;
    }
    public GameObject ItemPrefab
    {
        get => _itemPrefab;
    }
}
