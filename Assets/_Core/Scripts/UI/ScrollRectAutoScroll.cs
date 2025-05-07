using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _scrollSpeed = 10f;

    private bool _mouseOver = false;
    private ScrollRect _scrollRect;
    private List<Selectable> _selectables = new List<Selectable>();
    private Vector2 _nextScrollPosition = Vector2.up;
    private Selectable _previousSelected;
    private GameObject _lastSelected = null;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    private void Start()
    {
        _scrollRect.content.GetComponentsInChildren(_selectables);
        _lastSelected = EventSystem.current.currentSelectedGameObject;
        ScrollToSelected(false);
    }

    private void Update()
    {
        if (!_mouseOver)
        {
            _scrollRect.normalizedPosition = Vector2.Lerp(
                _scrollRect.normalizedPosition,
                _nextScrollPosition,
                _scrollSpeed * Time.unscaledDeltaTime
            );
        }
        else
        {
            _nextScrollPosition = _scrollRect.normalizedPosition;
        }

        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current != _lastSelected)
        {
            _lastSelected = current;
            ScrollToSelected(false);
        }
    }

    public void InputScroll(InputAction.CallbackContext context)
    {
        if (!context.performed || _selectables.Count == 0)
            return;

        Vector2 inputDir = context.ReadValue<Vector2>();

        if (Mathf.Abs(inputDir.x) > 0.5f || Mathf.Abs(inputDir.y) < 0.1f)
            return;
    }


    private void ScrollToSelected(bool instant)
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            return;

        Selectable current = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        if (current == null || !_selectables.Contains(current))
            return;

        RectTransform currentRect = current.GetComponent<RectTransform>();
        RectTransform viewport = _scrollRect.viewport;

        bool sameLine = false;
        
        if (_previousSelected != null && _previousSelected != current)
        {
            RectTransform previousRect = _previousSelected.GetComponent<RectTransform>();
            float verticalDistance = Mathf.Abs(currentRect.position.y - previousRect.position.y);
            sameLine = verticalDistance == 0;
        }

        _previousSelected = current;

        if (sameLine)
            return;

        Vector3[] itemCorners = new Vector3[4];
        Vector3[] viewportCorners = new Vector3[4];
        currentRect.GetWorldCorners(itemCorners);
        viewport.GetWorldCorners(viewportCorners);

        float itemTop = itemCorners[1].y;
        float itemBottom = itemCorners[0].y;
        float viewportTop = viewportCorners[1].y;
        float viewportBottom = viewportCorners[0].y;

        if (itemTop <= viewportTop && itemBottom >= viewportBottom)
            return;

        float index = _selectables.IndexOf(current) / 5 + 1;
        float normalizedY = 1 - ((index - 1) / 5);
        
        if (normalizedY < .5f)
        {
            normalizedY = .17f;
        }
        else
        {
            normalizedY = 1;
        }
        
        Vector2 targetPos = new Vector2(0, normalizedY);

        if (instant)
        {
            _scrollRect.normalizedPosition = targetPos;
        }

        _nextScrollPosition = targetPos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseOver = false;
        ScrollToSelected(false);
    }
}