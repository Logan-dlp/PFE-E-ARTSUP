using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectAutoScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scrollSpeed = 10f;
    private bool mouseOver = false;

    private ScrollRect scrollRect;
    private List<Selectable> selectables = new List<Selectable>();
    private Vector2 nextScrollPosition = Vector2.up;

    private GameObject lastSelected = null;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    private void Start()
    {
        scrollRect.content.GetComponentsInChildren(selectables);
        lastSelected = EventSystem.current.currentSelectedGameObject;
        ScrollToSelected(true);
    }

    private void Update()
    {
        if (!mouseOver)
        {
            scrollRect.normalizedPosition = Vector2.Lerp(
                scrollRect.normalizedPosition,
                nextScrollPosition,
                scrollSpeed * Time.unscaledDeltaTime
            );
        }
        else
        {
            nextScrollPosition = scrollRect.normalizedPosition;
        }

        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current != lastSelected)
        {
            lastSelected = current;
            ScrollToSelected(false);
        }
    }

    public void InputScroll(InputAction.CallbackContext context)
    {
        if (!context.performed || selectables.Count == 0)
            return;

        Vector2 inputDir = context.ReadValue<Vector2>();

        if (Mathf.Abs(inputDir.x) > 0.5f || Mathf.Abs(inputDir.y) < 0.1f)
            return;
    }

    private Selectable m_PreviousSelected;

    private void ScrollToSelected(bool instant)
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            return;

        Selectable current = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
        if (current == null || !selectables.Contains(current))
            return;

        RectTransform currentRect = current.GetComponent<RectTransform>();
        RectTransform viewport = scrollRect.viewport;

        bool sameLine = false;
        if (m_PreviousSelected != null && m_PreviousSelected != current)
        {
            RectTransform previousRect = m_PreviousSelected.GetComponent<RectTransform>();
            float verticalDistance = Mathf.Abs(currentRect.position.y - previousRect.position.y);
            sameLine = verticalDistance < 1f;
        }

        m_PreviousSelected = current;

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

        int index = selectables.IndexOf(current);
        float normalizedY = 1f - (index / (float)(selectables.Count - 1));
        Vector2 targetPos = new Vector2(0, normalizedY);

        if (instant)
        {
            scrollRect.normalizedPosition = targetPos;
        }

        nextScrollPosition = targetPos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
        ScrollToSelected(false);
    }
}