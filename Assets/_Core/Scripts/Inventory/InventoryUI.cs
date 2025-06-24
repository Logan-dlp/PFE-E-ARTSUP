using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoonlitMixes.Datas;
using MoonlitMixes.Item;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitMixes.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public GameObject FirstSelected { get; private set; }
        
        [SerializeField] private ItemData _emptyItem;
        public ItemData EmptyData => _emptyItem;
        
        private int _emptySlot;
        public int EmptySlot => _emptySlot;
        
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private List<InventoryData> _inventoryExtensionList;
        [SerializeField] private InventoryData _inventoryReceives;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Vector3 _scaleItem;
        
        private void OnEnable()
        {
            RefreshInventory();
        }

        public void TransfersInventory()
        {
            foreach (InventoryData inventoryData in _inventoryExtensionList)
            {
                if (inventoryData.Items != null && inventoryData.Items.Count != 0)
                {
                    foreach (ItemData itemData in inventoryData.Items)
                    {
                        if (itemData.name != "Empty")
                        {
                            _inventory.Items.Add(itemData);
                            inventoryData.Items.Remove(itemData);
                        }
                    }
                }
            }
        }

        public void RefreshInventory()
        {
            InventoryData allInventory = ScriptableObject.CreateInstance<InventoryData>();

            foreach (InventoryData inventoryData in _inventoryExtensionList)
            {
                foreach (ItemData itemData in inventoryData.Items)
                {
                    if (itemData.name != "Empty")
                    {
                        allInventory.Items.Add(itemData);
                    }
                }
            }

            foreach (ItemData itemData in _inventory.Items)
            {
                allInventory.Items.Add(itemData);
            }

            allInventory = SortInventory(allInventory);
            
            _emptySlot = 0;
            if (_inventory.name != "Inventory Cellar")
            {
                for (int i = 0; i < _inventory.Items.Count; i++)
                {
                    if (_inventory.Items[i] == null) _inventory.Items[i] = _emptyItem;
                    else if (_inventory.Items[i].name == "Empty")
                    {
                        _emptySlot++;
                    }
                }
            }
            
            foreach (Transform childTransform in transform)
            {
                Destroy(childTransform.gameObject);
            }
            
            List<GameObject> currentItemList = new();
            
            foreach (ItemData currentItemData in allInventory.Items)
            {
                GameObject itemCase = Instantiate(_slotPrefab, transform);
                currentItemList.Add(itemCase);
                itemCase.name = $"Slot_{currentItemList.IndexOf(itemCase)}";

                GameObject item = new GameObject("Item");
                item.transform.SetParent(itemCase.transform);
                
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = _scaleItem;
                item.transform.localRotation = Quaternion.identity;
                Image itemImage = item.AddComponent<Image>();
                itemImage.sprite = currentItemData.ItemSprite;
                itemImage.preserveAspect = true;
                itemImage.rectTransform.sizeDelta = new Vector2(100, 100);

                ItemDataHolder itemDataHolder = item.AddComponent<ItemDataHolder>();
                itemDataHolder.ItemData = currentItemData;
            }

            FirstSelected = currentItemList.FirstOrDefault();
            if (_inventory.name != "Inventory Cellar")
            {
                while (_inventory.Items.Count < _inventory.MaxSlots)
                {
                    _inventory.Items.Add(_emptyItem);
                }
                /*EventSystem.current.SetSelectedGameObject(FirstSelected);
                EventSystem.current.firstSelectedGameObject = FirstSelected;*/
            }
            else StartCoroutine(SelectedButton());
        }
        
        public IEnumerator SelectedButton()
        {
            yield return new WaitForSeconds(0.1f);
            EventSystem.current.SetSelectedGameObject(FirstSelected);
            EventSystem.current.firstSelectedGameObject = FirstSelected;
        }
        
        public void AddItem(ItemData item)
        {
            if (item == null)
            {
                Debug.LogWarning("L'item � ajouter est nul !");
                return;
            }

            for(int i = 0; i < _inventory.Items.Count; i++)
            {
                if (item.name == "Empty")
                {
                    break;
                }
                
                if (_inventory.Items[i].name == "Empty" )
                {
                    _inventory.Items[i] = item;
                    break;
                }
            }
            
            RefreshInventory();
            Debug.Log($"{item.name} ajout� avec succ�s !");
        }

        public void RemoveItem(ItemData item)
        {
            if (item == null)
            {
                Debug.LogWarning("L'item � supprimer est nul !");
                return;
            }

            if (_inventory.Items.Contains(item))
            {
                _inventory.Items.Remove(item);
                RefreshInventory();
                Debug.Log($"{item.name} d�truit avec succ�s !");
            }
            else
            {
                Debug.LogWarning("L'objet n'existe pas dans l'inventaire !");
            }
        }

        private InventoryData SortInventory(InventoryData inventory)
        {
            inventory.Items = inventory.Items.OrderBy(item => item.Type)
                                                    .ThenBy(item => item.Rarity)
                                                    .ToList();

            return inventory;
        }

        public IReadOnlyList<ItemData> Items => _inventory.Items.AsReadOnly();

        public bool ContainsItem(ItemData item)
        {
            return _inventory.Items.Contains(item);
        }

        public void SendItems()
        {
            try
            {
                for (int i = _inventory.Items.Count - 1; i >= 0; i--)
                {
                    ItemData item = _inventory.Items[i];
                    int emptyInventoryR = 0;
                    for (int j = 0; j < _inventoryReceives.Items.Count; j++)
                    {
                        if (_inventoryReceives.Items[j].name == "Empty")
                        {
                            emptyInventoryR++;
                        }
                    }
                    if (emptyInventoryR>= _inventory.Items.Count - _emptySlot)
                    {
                        if (item.name != "Empty")
                        {
                            for (int j = 0; j < _inventoryReceives.Items.Count; j++)
                            {
                                if (_inventoryReceives.Items[j].name == "Empty")
                                {
                                    _inventoryReceives.Items[j] = item;
                                    _inventory.Items[i] = _emptyItem;
                                    break;
                                }
                            }
                            RefreshInventory();

                            Debug.Log("Envoie des items dans l'inventaire destin�");
                        }
                        
                    }
                    else
                    {
                        Debug.Log(emptyInventoryR);
                        Debug.Log(_inventory.Items.Count - _emptySlot);
                        Debug.LogWarning("L'inventaire destin� est plein");
                        break;
                    }
                }
            }
            catch (System.Exception error)
            {
                Debug.LogError($"Error SendItems in InventoryUI : {error.Message}");
            }
            RefreshInventory();
        }
    }
}