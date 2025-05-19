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
        [SerializeField] private int _maxLinePerVue = 3;
        [SerializeField] private int _maxItemPerVue = 15;
        [SerializeField] private float _transitionSpeed = 0.005f;
        
        private ScrollRect _scrollRect;
        private RectTransform _contentRect;
        private GridLayoutGroup _gridLayoutGroup;
        private List<GameObject> _cellarItemList = new();
        private GameObject _currentSelectedItem;

        private float _currentScrollBarValue = 0;

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _gridLayoutGroup = GetComponentInChildren<GridLayoutGroup>();
            _contentRect = _gridLayoutGroup.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_currentSelectedItem == null || EventSystem.current.currentSelectedGameObject != _currentSelectedItem && _cellarItemList.Contains(EventSystem.current.currentSelectedGameObject))
            {
                _currentSelectedItem = EventSystem.current.currentSelectedGameObject;
                UpdateScroller();
            }
            
            _scrollRect.verticalScrollbar.value = Mathf.Lerp(_scrollRect.verticalScrollbar.value, _currentScrollBarValue, Time.unscaledTime * _transitionSpeed);
        }
        
        private void UpdateScroller()
        {
            RefreshCellarItems();

            float maxVue = _cellarItemList.Count / (float)_maxItemPerVue;
            if (maxVue % 1 > 0)
            {
                maxVue = maxVue + 1 - maxVue % 1;
            }
            
            float currentVue = (_cellarItemList.IndexOf(_currentSelectedItem) + 1) / (float)_maxItemPerVue;
            if (currentVue % 1 > 0)
            {
                currentVue = currentVue + 1 - currentVue % 1;
            }
            
            _currentScrollBarValue = 1 - ((currentVue - 1) / (maxVue - 1));
        }
        
        private void RefreshCellarItems()
        {
            if (_cellarItemList != null || _cellarItemList.Count > 0) 
                _cellarItemList.Clear();
            
            GameObject parentContent = GetComponentInChildren<InventoryUI>().gameObject;
            foreach (Transform children  in parentContent.transform)
            {
                _cellarItemList.Add(children.gameObject);
            }
            
            float maxVue = _cellarItemList.Count / (float)_maxItemPerVue;
            if (maxVue % 1 > 0)
            {
                maxVue = maxVue + 1 - maxVue % 1;
            }
            
            _contentRect.sizeDelta = new Vector2(_contentRect.sizeDelta.x, ((_gridLayoutGroup.cellSize.y + _gridLayoutGroup.spacing.y) * _maxLinePerVue) * maxVue + _gridLayoutGroup.spacing.y);
        }
    }
}