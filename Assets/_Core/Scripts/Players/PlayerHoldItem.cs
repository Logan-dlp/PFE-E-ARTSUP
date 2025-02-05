using System;
using MoonlitMixes.Events;
using MoonlitMixes.Item;
using UnityEngine;

namespace MoonlitMixes.Player
{
    public class PlayerHoldItem : MonoBehaviour
    {
        [SerializeField] private GameObject _itemHoldPivot;
        [SerializeField] private ScriptableItemEvent _scriptableItemEvent;
    
        public ItemData Item { get; set; }
        public GameObject ItemHold { get; set; }
    
        private void OnEnable()
        {
            _scriptableItemEvent.ItemDataAction += ChangeItemData;
        }
    
        private void OnDisable()
        {
            _scriptableItemEvent.ItemDataAction -= ChangeItemData;
        }
    
        private void DisplayItemHold()
        {
            ItemHold.transform.localPosition = Vector3.zero;
            ItemHold.transform.localScale = Vector3.one;
        }
        
        
        public void ChangeItemData(GameObject item)
        {
            try
            {
                foreach (Transform child in _itemHoldPivot.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            catch
            {
                Debug.Log("No childs");
                throw;
            }

            ItemHold = Instantiate(item, _itemHoldPivot.transform);
            Item = ItemHold.GetComponent<ItemDataHolder>().ItemData;
            DisplayItemHold();
            GetComponent<PlayerInteraction>().ItemInHand = ItemHold;
        }

        public void RemoveItem()
        {
            try
            {
                foreach (Transform child in _itemHoldPivot.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            catch
            {
                Debug.Log("No childs");
                throw;
            }

            ItemHold = null;
            Item = null;
            GetComponent<PlayerInteraction>().ItemInHand = null;
        }
    }
}