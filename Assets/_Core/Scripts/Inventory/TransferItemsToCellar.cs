using UnityEngine;

public class TransferItemsToCellar : MonoBehaviour
{
    [SerializeField] private InventoryData _inventoryPlayer;
    [SerializeField] private InventoryData _inventoryCellar;

    private InventoryUI _inventoryUI;

    private void Awake()
    {
        _inventoryUI = GetComponent<InventoryUI>();
    }

    public void SendItems()
    {
        if (_inventoryPlayer == null || _inventoryCellar == null)
        {
            Debug.LogWarning("Inventaire joueur ou cave non défini !");
            return;
        }

        for (int i = _inventoryPlayer.Items.Count - 1; i >= 0; i--)
        {
            ItemData item = _inventoryPlayer.Items[i];

            _inventoryCellar.Items.Add(item);
            _inventoryPlayer.Items.RemoveAt(i);
            Debug.Log("Ajout des items dans l'inventaire de Cellar !");
        }

        _inventoryUI.RefreshUI();
    }
}