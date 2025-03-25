using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    public class ScrollViewSample : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private GameObject _prefabListItem;
    
        [SerializeField] private ItemButtonEvent _eventItemClicked;
        [SerializeField] private ItemButtonEvent _eventItemSelect;
        [SerializeField] private ItemButtonEvent _eventItemSubmit;
    
        [SerializeField] private int _defaultSelectedIndex = 0;
    
        void OnEnable()
        {
            GetButtons();
        }
    
        private void GetButtons()
        {
            ItemButton[] itemArray = FindObjectsByType<ItemButton>(FindObjectsSortMode.InstanceID);
            Array.Reverse(itemArray);
    
            foreach (ItemButton item in itemArray)
            {
                item.onSelectEvent.AddListener(item => HandleEventItemOnSelect(item));
                item.onClickEvent.AddListener(item => HandleEventItemOnClick(item));
                item.onSubmitEvent.AddListener(item => HandleEventItemOnSubmit(item));
                UpdateAllButtonNavReferences(itemArray);
            }
            StartCoroutine(DelayedSelectChild(_defaultSelectedIndex));
        }
    
        public void SelectChild(int index)
        {
            int childCount = _content.transform.childCount;
            if (index >= childCount) return;
    
            GameObject childObject = _content.transform.GetChild(index).gameObject;
            ItemButton item = childObject.GetComponent<ItemButton>();
            item.ObtainSelectionFocus();
        }
    
        public IEnumerator DelayedSelectChild(int index)
        {
            yield return new WaitForSeconds(1f);
            SelectChild(index);
        }
    
        private void UpdateAllButtonNavReferences(ItemButton[] itemArray)
        {
            if (itemArray.Length < 2) return;
    
            ItemButton item;
            Navigation localNavigation;
    
            for (int i = 0; i < itemArray.Length; i++)
            {
                item = itemArray[i];
                localNavigation = item.gameObject.GetComponent<Button>().navigation;
    
                localNavigation.selectOnUp = GetNavigationUp(i, itemArray);
                localNavigation.selectOnDown = GetNavigationDown(i, itemArray);
                localNavigation.selectOnLeft = GetNavigationLeft(i, itemArray);
                localNavigation.selectOnRight = GetNavigationRight(i, itemArray);
    
                item.gameObject.GetComponent<Button>().navigation = localNavigation;
            }
        }
    
        private Selectable GetNavigationUp(int i, ItemButton[] itemArray)
        {
            return (itemArray.Length < 5 || i < 5) ? null : itemArray[i - 5].GetComponent<Selectable>();
        }
    
        private Selectable GetNavigationDown(int i, ItemButton[] itemArray)
        {
            return (itemArray.Length < 5 || i >= itemArray.Length - 5) ? null : itemArray[i + 5].GetComponent<Selectable>();
        }
    
        private Selectable GetNavigationLeft(int i, ItemButton[] itemArray)
        {
            return (itemArray.Length < 2 || i % 5 == 0) ? null : itemArray[i - 1].GetComponent<Selectable>();
        }
    
        private Selectable GetNavigationRight(int i, ItemButton[] itemArray)
        {
            return (itemArray.Length < 2 || i % 5 == 4) ? null : itemArray[i + 1].GetComponent<Selectable>();
        }
    
        private void HandleEventItemOnSelect(ItemButton item)
        {
            _eventItemSelect.Invoke(item);
            AutoScroll autoScroll = GetComponent<AutoScroll>();
            autoScroll.HandleOnSelectChange(item.gameObject);
        }
    
        private void HandleEventItemOnClick(ItemButton item)
        {
            _eventItemClicked.Invoke(item);
        }
    
        private void HandleEventItemOnSubmit(ItemButton item)
        {
            _eventItemSubmit.Invoke(item);
        }
    }
}
