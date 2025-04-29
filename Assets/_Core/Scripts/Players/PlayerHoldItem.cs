using UnityEngine;
using MoonlitMixes.Events;
using MoonlitMixes.Item;
using System.Collections;

namespace MoonlitMixes.Player
{
    public class PlayerHoldItem : MonoBehaviour
    {
        public ItemData Item { get; set; }
        public GameObject ItemHold { get; set; }
        
        [SerializeField] private GameObject _itemHoldPivot;
        [SerializeField] private ScriptableItemEvent _scriptableItemEvent;
        [SerializeField] private ScriptableItemUsageEvent _scriptableItemUsageEvent;
        
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

            ItemHold = Instantiate(item, _itemHoldPivot.transform.position, item.transform.rotation, _itemHoldPivot.transform);

            Item = ItemHold.GetComponent<ItemDataHolder>().ItemData;
            DisplayItemHold();
            _scriptableItemUsageEvent.SendEvent(Item.Usage);
            GetComponent<PlayerInteraction>().ItemInHand = Item;
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