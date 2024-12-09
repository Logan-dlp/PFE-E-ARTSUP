using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryData _inventory;
    [SerializeField] private InventoryData _inventoryReceives;
    [SerializeField] private GameObject _slotPrefab;

    private int _initialSlotsPerRow = 5;

    private void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        int itemCount = _inventory.Items.Count;
        int slotCount = transform.childCount;

        while (slotCount < itemCount)
        {
            AddNewRowOfSlots();
            slotCount = transform.childCount;
        }

        for (int i = 0; i < slotCount; i++)
        {
            Transform slot = transform.GetChild(i);

            if (i < itemCount)
            {
                ItemData item = _inventory.Items[i];
                Transform itemTransform = slot.Find("Item");

                if (itemTransform == null)
                {
                    GameObject itemObj = new GameObject("Item");
                    itemObj.transform.SetParent(slot);
                    itemObj.transform.localPosition = Vector3.zero;

                    Image itemImage = itemObj.AddComponent<Image>();
                    itemImage.sprite = item.ItemSprite;
                    itemImage.rectTransform.sizeDelta = new Vector2(100, 100);
                }
                else
                {
                    Image itemImage = itemTransform.GetComponent<Image>();
                    if (itemImage != null)
                    {
                        itemImage.sprite = item.ItemSprite;
                    }
                }
            }
            else
            {
                Transform itemTransform = slot.Find("Item");
                if (itemTransform != null)
                {
                    Destroy(itemTransform.gameObject);
                }
            }
        }
    }

    private void AddNewRowOfSlots()
    {
        for (int i = 0; i < _initialSlotsPerRow; i++)
        {
            GameObject newSlot = Instantiate(_slotPrefab, transform);
            newSlot.name = "Slot_" + (transform.childCount + 1);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("L'item à ajouter est nul !");
            return;
        }

        if (_inventory.Mode == InventoryData.InventoryMode.InventoryPlayer && _inventory.Items.Count >= _inventory.MaxSlots)
        {
            Debug.LogWarning("L'inventaire est plein !");
            return;
        }

        _inventory.Items.Add(item);
        SortInventory();
        RefreshUI();
        Debug.Log($"{item.name} ajouté avec succès !");
    }

    public void RemoveItem(ItemData item)
    {
        if (item == null)
        {
            Debug.LogWarning("L'item à supprimer est nul !");
            return;
        }

        if (_inventory.Items.Contains(item))
        {
            _inventory.Items.Remove(item);
            RefreshUI();
            Debug.Log($"{item.name} détruit avec succès !");
        }
        else
        {
            Debug.LogWarning("L'objet n'existe pas dans l'inventaire !");
        }
    }

    private void SortInventory()
    {
        _inventory.Items = _inventory.Items
            .OrderBy(item => item.Type)
            .ThenBy(item => item.Rarity)
            .ToList();
    }

    public IReadOnlyList<ItemData> Items => _inventory.Items.AsReadOnly();

    public bool ContainsItem(ItemData item)
    {
        return _inventory.Items.Contains(item);
    }

    public void SendItems(InventoryData inventoryData)
    {
        try
        {
            for (int i = _inventory.Items.Count - 1; i >= 0; i--)
            {
                ItemData item = _inventory.Items[i];

                if (_inventoryReceives.Items.Count < _inventoryReceives.MaxSlots)
                {
                    _inventoryReceives.Items.Add(item);
                    _inventory.Items.RemoveAt(i);
                    Debug.Log("Envoie des items dans l'inventaire destiné");
                }
                else
                {
                    Debug.LogWarning("L'inventaire destiné est plein");
                    break;
                }
            }
        }
        catch (System.Exception error)
        {
            Debug.LogError($"Error SendItems in InventoryUI : {error.Message}");
        }

        RefreshUI();
    }
}