using UnityEngine;

public class TransferItemsToOtherInventory : MonoBehaviour
{
    [SerializeField] private InventoryData _inventorySends;
    [SerializeField] private InventoryData _inventoryReceives;

    private InventoryUI _inventoryUIPlayer;

    private void Awake()
    {
        _inventoryUIPlayer = GetComponent<InventoryUI>();

        if (_inventoryUIPlayer == null)
        {
            Debug.Log("Il manque le script Inventory UI");
        }
    }

    public void SendItems()
    {
        if (_inventorySends == null || _inventoryReceives == null)
        {
            Debug.LogWarning("Inventaire joueur ou cave non défini !");
            return;
        }

        for (int i = _inventorySends.Items.Count - 1; i >= 0; i--)
        {
            ItemData item = _inventorySends.Items[i];

            _inventoryReceives.Items.Add(item);
            _inventorySends.Items.RemoveAt(i);
            Debug.Log("Ajout des items dans l'inventaire de Cellar !");
        }

        _inventoryUIPlayer.RefreshUI();
    }
}