using UnityEngine;

public class ProtoItemGiver : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    public ItemData GiveItem()
    {
        return itemData;
    }
}
