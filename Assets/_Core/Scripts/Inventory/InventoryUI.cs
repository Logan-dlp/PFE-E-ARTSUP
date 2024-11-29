using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static InventoryData;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryData _inventory;
    [SerializeField] private GameObject _inventorySlotPrefab;

    private void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemData item in _inventory.Items)
        {
            GameObject slot = Instantiate(_inventorySlotPrefab, transform);
            Image icon = slot.GetComponentInChildren<Image>();
            icon.sprite = item.ItemSprite;
        }
    }

    public bool CanAddItem()
    {
        if (_inventory.Mode == InventoryMode.InventoryPlayer)
        {
            return _inventory.Items.Count < _inventory.MaxSlots;
        }
        return true;
    }

    public void OnAddItemButtonClicked(ItemData item)
    {
        if (AddItem(item))
        {
            Debug.Log("Item ajouté avec succès !");
        }
    }

    public void OnRemovedItemButtonClicked(ItemData item)
    {
        if (RemoveItem(item))
        {
            Debug.Log("Item détruit avec succès !");
        }
    }


    public bool AddItem(ItemData item)
    {
        if (!CanAddItem())
        {
            Debug.LogWarning("L'inventaire est plein !");
            return false;
        }
        else
        {
            _inventory.Items.Add(item);
            SortInventory();
            RefreshUI();
            return true;
        }
    }

    public bool RemoveItem(ItemData item)
    {
        if (_inventory.Items.Contains(item))
        {
            _inventory.Items.Remove(item);
            RefreshUI();
            return true;
        }
        Debug.LogWarning("L'objet n'existe pas dans l'inventaire !");
        return false;
    }

    private void SortInventory()
    {
        _inventory.Items = _inventory.Items.OrderBy(item => item.Rarity).ToList();
    }

    public IReadOnlyList<ItemData> Items => _inventory.Items.AsReadOnly();

    public bool ContainsItem(ItemData item)
    {
        return _inventory.Items.Contains(item);
    }
}