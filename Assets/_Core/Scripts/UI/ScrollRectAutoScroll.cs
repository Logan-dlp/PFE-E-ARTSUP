using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    using Inventory;
    
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectAutoScroll : MonoBehaviour
    {
        private const int _maxItemPerVue = 15;
        
        private ScrollRect _scrollRect;
        private List<GameObject> _cellarItemList = new();
        private GameObject _currentSelectedItem;

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        private void Update()
        {
            if (_currentSelectedItem == null || EventSystem.current.currentSelectedGameObject != _currentSelectedItem && _cellarItemList.Contains(EventSystem.current.currentSelectedGameObject))
            {
                _currentSelectedItem = EventSystem.current.currentSelectedGameObject;
                UpdateScroller();
            }
        }
        
        private void UpdateScroller()
        {
            RefreshCellarItems();
            
            float maxVueNumber = _cellarItemList.Count / (float)(_maxItemPerVue - 1);
            maxVueNumber -= maxVueNumber % 1;
            
            float currentVueNumber = (float)(_cellarItemList.IndexOf(_currentSelectedItem) + 1) / _maxItemPerVue;
            currentVueNumber -= currentVueNumber % 1;
            
            _scrollRect.verticalScrollbar.value = 1 - (currentVueNumber / maxVueNumber);
            // _scrollRect.normalizedPosition = new Vector2(_scrollRect.normalizedPosition.x, (currentVueNumber / maxVueNumber) + 1);
        }

        /// <summary>
        /// Refresh element to make Auto Scroll
        /// </summary>
        public void RefreshCellarItems()
        {
            if (_cellarItemList != null || _cellarItemList.Count > 0) 
                _cellarItemList.Clear();
            
            GameObject parentContent = GetComponentInChildren<InventoryUI>().gameObject;
            foreach (Transform children  in parentContent.transform)
            {
                _cellarItemList.Add(children.gameObject);
            }
        }
    }
}