using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string obejctName;
    [SerializeField] private ElementType elementType;
    [SerializeField, Range(1,4)] private int rarity;
    [SerializeField] private ItemUsage itemUsage;
    [SerializeField] private ItemUsage state;
    [SerializeField] private Sprite sprite;
    [SerializeField] private ItemData itemToConvert;
    [SerializeField] private string description;
    
    public string ObjectName
    {
        get => obejctName;
    }

    public ElementType Type
    {
        get => elementType;
    }

    public int Rarity
    {
        get => rarity;
    }

    public ItemUsage Usage
    {
        get => itemUsage;
    }

    public ItemUsage State
    {
        get => state;
    }

    public Sprite ItemSprite
    {
        get => sprite;
    }
    
    public ItemData ItemToConvert
    {
        get => itemToConvert;
    } 

    private string Description
    {
        get => description;
    }
}
