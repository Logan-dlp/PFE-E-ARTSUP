using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryData _inventory;
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

    public bool CanAddItem()
    {
        if (_inventory.Mode == InventoryData.InventoryMode.InventoryPlayer)
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