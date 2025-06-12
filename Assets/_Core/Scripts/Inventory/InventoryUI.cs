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
        
        [SerializeField] private InventoryData _inventory;
        [SerializeField] private InventoryData _inventoryReceives;
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Vector3 _scaleItem;
        [SerializeField] private ItemData _emptyItem;
        public ItemData EmptyData { get { return _emptyItem; } }
        private int _emptySlot;
        public int EmptySlot { get { return _emptySlot; } }
        private void OnEnable()
        {
            RefreshInventory();
        }

        public void RefreshInventory()
        {
            _emptySlot = 0;
            for (int i = 0; i < _inventory.Items.Count; i++)
            {
                if (_inventory.Items[i] == null) _inventory.Items[i] = _emptyItem;
                else if(_inventory.Items[i].name == "Empty")
                {
                    _emptySlot++;
                }
            }
            SortInventory();
            
            foreach (Transform childTransform in transform)
            {
                Destroy(childTransform.gameObject);
            }
            
            List<GameObject> currentItemList = new();
            
            foreach (ItemData currentItemData in _inventory.Items)
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
            _emptySlot = 0;
            for (int i = 0; i < _inventory.Items.Count; i++)
            {
                if (_inventory.Items[i].name == "Empty")
                {
                    _emptySlot++;
                }
                else if (_inventory.Items[i] == null) _inventory.Items[i] = _emptyItem;
            }
            while (_inventory.Items.Count < _inventory.MaxSlots)
            {
                _inventory.Items.Add(_emptyItem);
            }
            EventSystem.current.SetSelectedGameObject(FirstSelected);
            EventSystem.current.firstSelectedGameObject = FirstSelected;
            SortInventory();

            //Debug.Log(gameObject.name + _emptySlot);
        }

        public void AddItem(ItemData item)
        {
            RefreshInventory();
            bool full=true;
            if (item == null)
            {
                Debug.LogWarning("L'item � ajouter est nul !");
                return;
            }

            
            for(int i = 0;i<_inventory.Items.Count;i++)
            {
                if (item.name == "Empty")
                {

                }
                else if (_inventory.Items[i].name == "Empty" )
                {
                    _inventory.Items[i] = item;
                    full=false;
                    break;
                }
            }
            /*if (_inventory.Mode == InventoryMode.InventoryPlayer && _inventory.Items.Count >= _inventory.MaxSlots)
            {
                Debug.LogWarning("L'inventaire est plein !");
                return;
            }*/
            if (full) { Debug.LogWarning("L'inventaire est plein !"); return; }
            SortInventory();
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

        public void SendItems()
        {
            try
            {
                for (int i = _inventory.Items.Count - 1; i >= 0; i--)
                {
                    ItemData item = _inventory.Items[i];
                    _emptySlot = 0;
                    int emptyInventoryR = 0;
                    for (int j = 0; j < _inventoryReceives.Items.Count; j++)
                    {
                        if (_inventoryReceives.Items[j].name == "Empty")
                        {
                            emptyInventoryR++;
                        }
                    }
                    if (emptyInventoryR> _inventory.Items.Count - _emptySlot)
                    {
                        if (item.name != "Empty")
                        {
                            /*_inventoryReceives.Items.Add(item);
                            _inventory.Items.RemoveAt(i);*/
                            _inventory.Items[i] = _emptyItem;
                           
                            if (_inventory.Items[i].name == "Empty")
                            {
                                _inventoryReceives.Items[i] = item;
                            }
                            Debug.Log("Envoie des items dans l'inventaire destin�");
                        }
                        
                    }
                    else
                    {
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