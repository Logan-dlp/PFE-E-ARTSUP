using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string obejctName;
    [SerializeField] private ElementType elementType;
    [SerializeField, Range(1,4)] private int rarity;
    [SerializeField] private ItemUsage itemUsage;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Item itemToTransform;
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

    public Sprite ItemSprite
    {
        get => sprite;
    }
    
    public Item ItemToTransform
    {
        get => itemToTransform;
    } 

    private string Description
    {
        get => description;
    }

    public Item TransformToOtherItem()
    {
        return itemToTransform;
    }
}