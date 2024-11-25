using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string obejctName;
    public ElementType elementType;
    [Range(1,4)] public int rarity;
    public ItemUsage itemUsage;
    public Sprite sprite;
    public Item itemToTransform;

    public Item TransformToOtherItem()
    {
        return itemToTransform;
    }
}