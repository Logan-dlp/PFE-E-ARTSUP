using MoonlitMixes.Datas;
using MoonlitMixes.Item;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MoonlitMixes.Inventory
{
    public class DiscardInventory : MonoBehaviour
    {
        [SerializeField] private InventoryData _playerInventory;
        [SerializeField] private InventoryData _chestInventory;
        [SerializeField] private InventoryUI _inventoryUI;
        [SerializeField] private InventoryUI _inventoryChestUI;
        [SerializeField] private GameObject _validationDiscard;
        private bool _canDiscard = false;
        public bool CanDiscard => _canDiscard;
        public InventoryUI InventoryUI { get { return _inventoryUI; } }

        public void DiscardBag()
        {
            if( _canDiscard )
            {
                string name = EventSystem.current.currentSelectedGameObject.name;
                bool result = char.IsDigit(name[name.Length - 1]);
                int selected = 0;
                if (char.IsDigit(name[name.Length - 2]) && result)
                {

                    int i = 0, y = 0;
                    i = (int)char.GetNumericValue(name, name.Length - 1);
                    y = (int)char.GetNumericValue(name, name.Length - 2);
                    y = y * 10 + i;
                    _playerInventory.Items[y] = _inventoryUI.EmptyData;
                    selected = y;
                }
                else if (result)
                {
                    int i = 0;
                    i = (int)char.GetNumericValue(name, name.Length - 1);
                    _playerInventory.Items[i] = _inventoryUI.EmptyData;
                    selected = i;
                }
                _inventoryUI.RefreshInventory();
                StartCoroutine(SetSelectedButtonBag(selected));
                _canDiscard = false;
                _validationDiscard.SetActive(false);
                EventSystem.current.sendNavigationEvents = true;

            }
        }
        public void Submit()
        {
            Debug.Log("Empty");
            if(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<ItemDataHolder>().ItemData.ObjectName!="Empty")
            {
                _canDiscard = true;
                _validationDiscard.SetActive(true);
                EventSystem.current.sendNavigationEvents = false;
            }
        }
        public void Cancel()
        {
            if (_canDiscard)
            {
                _canDiscard = false;
                EventSystem.current.sendNavigationEvents = true;
                _validationDiscard.SetActive(false);
            }
            _inventoryUI.RefreshInventory();
            StartCoroutine(SetSelectedButtonBag(selected));
            if(GetComponent<UseTools>().BagIsFull.activeInHierarchy) GetComponent<UseTools>().BagIsFull.SetActive(false);
        }
        private IEnumerator SetSelectedButtonBag(int i)
        {
            yield return new WaitForSeconds(0.1f);
            EventSystem.current.SetSelectedGameObject(_inventoryUI.transform.GetChild(i).gameObject);
            EventSystem.current.firstSelectedGameObject = _inventoryUI.transform.GetChild(i).gameObject;
        }
        public void DiscardChest()
        {
            if (_canDiscard) 
            {
                string name = EventSystem.current.currentSelectedGameObject.name;
                Boolean result = char.IsDigit(name[name.Length - 1]);
                int selected = 0;
                if (char.IsDigit(name[name.Length - 2]) && result)
                {
                    int i = 0, y = 0;
                    i = (int)char.GetNumericValue(name, name.Length - 1);
                    y = (int)char.GetNumericValue(name, name.Length - 2);
                    y = y * 10 + i;
                    _chestInventory.Items[y] = _inventoryChestUI.EmptyData;
                    selected = y;
                }
                else if (result)
                {
                    int i = 0;
                    i = (int)char.GetNumericValue(name, name.Length - 1);
                    _chestInventory.Items[i] = _inventoryChestUI.EmptyData;
                    selected = i;
                }
                _inventoryChestUI.RefreshInventory();
                StartCoroutine(SetSelectedButtonChest(selected));
                _canDiscard = false;
                _validationDiscard.SetActive(false);
                EventSystem.current.sendNavigationEvents = true;

            }

        }
        private IEnumerator SetSelectedButtonChest(int i)
        {
            yield return new WaitForSeconds(0.1f);
            EventSystem.current.SetSelectedGameObject(_inventoryChestUI.transform.GetChild(i).gameObject);
            EventSystem.current.firstSelectedGameObject = _inventoryChestUI.transform.GetChild(i).gameObject;
        }
    }
}
