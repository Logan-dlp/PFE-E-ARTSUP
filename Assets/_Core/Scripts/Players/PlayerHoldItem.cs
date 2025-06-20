using MoonlitMixes.CookingMachine;
using MoonlitMixes.Datas;
using MoonlitMixes.Events;
using MoonlitMixes.Item;
using System.Collections.Generic;
using UnityEngine;

namespace MoonlitMixes.Player
{
    public enum OutlineName
    {
        Cauldron1,
        Cauldron2,
        Cauldron3,
        CuttingTable,
        Mortar,
        WaitingTable,
        Cellar,
        Ladder,
        Crow
    }
    public class PlayerHoldItem : MonoBehaviour
    {
        

        public ItemData Item { get; set; }
        public GameObject ItemHold { get; set; }
        [SerializeField] private List<GameObject> _cauldronsOutline;
        [SerializeField] private GameObject _tableOutline;
        [SerializeField] private GameObject _cuttingBoardOutline;
        [SerializeField] private GameObject _ladderOutline;
        [SerializeField] private GameObject _cellarOutline;
        [SerializeField] private GameObject _mortarOutline;
        [SerializeField] private GameObject _crowOutline;
        public List<GameObject> CauldronsOutline => _cauldronsOutline;
        public GameObject TableOutline => _tableOutline;
        public GameObject CuttingBoardOutline => _cuttingBoardOutline;
        public GameObject LadderOutline => _ladderOutline;
        public GameObject CellarOutline => _cellarOutline;
        public GameObject CrowOutline => _crowOutline;
        public GameObject MortarOutline => _mortarOutline;
        [SerializeField] private GameObject _itemHoldPivot;
        [SerializeField] private ScriptableItemEvent _scriptableItemEvent;
        [SerializeField] private ScriptableItemUsageEvent _scriptableItemUsageEvent;
        [SerializeField] private WaitingTable _waitingTable;
        
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
        public void ActivateOutline(OutlineName outlineName)
        {
            Debug.Log("Activate : " + outlineName);
            Debug.Log(!TableIsFull());
            switch (outlineName)
            {
                case OutlineName.Cauldron1:
                    if (Item != null) if (Item.Usage == ItemUsage.Stir|| Item.Usage == ItemUsage.Whole) _cauldronsOutline[0].SetActive(true);
                    break;
                case OutlineName.Cauldron2:
                    if (Item != null) if (Item.Usage == ItemUsage.Stir || Item.Usage == ItemUsage.Whole) _cauldronsOutline[1].SetActive(true);
                    break;
                case OutlineName.Cauldron3:
                    if (Item != null) if (Item.Usage == ItemUsage.Stir || Item.Usage == ItemUsage.Whole) _cauldronsOutline[2].SetActive(true);
                    break;
                case OutlineName.CuttingTable:
                    if (Item != null) if (Item.Usage == ItemUsage.Cut) _cuttingBoardOutline.SetActive(true);
                    break;
                case OutlineName.Mortar:
                    if (Item != null) if (Item.Usage == ItemUsage.Crush) _mortarOutline.SetActive(true);
                    break;
                case OutlineName.WaitingTable: 
                    if (Item != null&& !TableIsFull() || Item == null && TableHaveItem()) _tableOutline.SetActive(true);
                    break;
                case OutlineName.Ladder:
                    if (Item == null) _ladderOutline.SetActive(true);
                    break;
                case OutlineName.Cellar:
                    if (Item != null && !Item.IsTransformed || Item == null) _cellarOutline.SetActive(true);
                    break;
                //Case crow prévu au cas où
                /*case OutlineName.Crow:
                    if (Item != null) _crowOutline.SetActive(true);
                    break;*/
                default:
                    Debug.LogWarning("Mauvaise Enum");
                    break;

            }
        }
        public void DeactivateOutline(GameObject gameObject)
        {
            gameObject.SetActive(false);
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

        public bool TryReturnItemToCellar(InventoryData cellarInventory)
        {
            if (Item == null)
            {
                Debug.LogWarning("No item in hand to return.");
                return false;
            }

            if (Item.IsTransformed)
            {
                Debug.Log("Item is transformed, cannot return to cellar inventory.");
                return false;
            }

            cellarInventory.Items.Add(Item);
            RemoveItem();

            return true;
        }
        private bool TableHaveItem()
        {
            for(int i =0;i<_waitingTable.ItemGameObjectArray.Length-1; i++)
            {
                if (_waitingTable.ItemGameObjectArray[i] != null) return true;
            }
            return false;
        }
        private bool TableIsFull()
        {
            for (int i = 0; i < _waitingTable.ItemGameObjectArray.Length - 1; i++)
            {
                if (_waitingTable.ItemGameObjectArray[i] == null) return false;
            }
            return true;
        }
    }
}