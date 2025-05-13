using MoonlitMixes.Datas;
using MoonlitMixes.Item;
using UnityEngine;

public class SendItemExit : MonoBehaviour
{
    [SerializeField] private InventoryData _inventory;
    [SerializeField] private InventoryData _inventoryReceives;
    
    public bool SendItems()
    {
        try
        {
            if(_inventory.Items.Count == 0)
            {
                return false;
            }
            else
            {
                for (int i = _inventory.Items.Count - 1; i >= 0; i--)
                {
                    ItemData item = _inventory.Items[i];
                    if (_inventoryReceives.Items.Count < _inventoryReceives.MaxSlots)
                    {
                        _inventoryReceives.Items.Add(item);
                        _inventory.Items.RemoveAt(i);
                        Debug.Log("Envoie des items dans l'inventaire destin�");
                    }
                    else
                    {
                        Debug.LogWarning("L'inventaire destin� est plein");
                        return false;
                    }
                }
                return true;
            }
        }
        catch (System.Exception error)
        {
            Debug.LogError($"Error SendItems in InventoryUI : {error.Message}");
            return false;
        }
    }
}
