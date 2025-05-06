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

    private List<Selectable> m_Selectables = new List<Selectable>();
    private ScrollRect m_ScrollRect;

    private Vector2 m_NextScrollPosition = Vector2.up;

    private void Awake()
    {
        m_ScrollRect = GetComponent<ScrollRect>();
    }

    private void Start()
    {
        if (m_ScrollRect)
        {
            m_ScrollRect.content.GetComponentsInChildren(m_Selectables);
        }
        ScrollToSelected(true);
    }

    private void OnEnable()
    {
        var inputAction = new InputAction("Navigate", binding: "<Gamepad>/leftStick");
        inputAction.performed += ctx => InputScroll(ctx);
        inputAction.Enable();
    }

    private void OnDisable()
    {
        // Unsubscribe when the script is disabled
        var inputAction = new InputAction("Navigate", binding: "<Gamepad>/leftStick");
        inputAction.performed -= ctx => InputScroll(ctx);
        inputAction.Disable();
    }

    private void Update()
    {
        // If the mouse is not over the scroll area, perform lerping scrolling.
        if (!mouseOver)
        {
            m_ScrollRect.normalizedPosition = Vector2.Lerp(m_ScrollRect.normalizedPosition, m_NextScrollPosition, scrollSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            m_NextScrollPosition = m_ScrollRect.normalizedPosition;
        }
    }

    public void InputScroll(InputAction.CallbackContext context)
    {
        if (m_Selectables.Count > 0)
        {
            // Read the joystick input value from the Input System (vertical movement)
            Vector2 direction = context.ReadValue<Vector2>();

            if (Mathf.Abs(direction.y) > 0.1f)  // Simple threshold to avoid too small joystick movements
            {
                ScrollToSelected(false);
            }
        }
    }

    private void ScrollToSelected(bool quickScroll)
    {
        int selectedIndex = -1;
        Selectable selectedElement = EventSystem.current.currentSelectedGameObject ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() : null;

        if (selectedElement)
        {
            selectedIndex = m_Selectables.IndexOf(selectedElement);
        }

        if (selectedIndex > -1)
        {
            if (quickScroll)
            {
                m_ScrollRect.normalizedPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
                m_NextScrollPosition = m_ScrollRect.normalizedPosition;
            }
            else
            {
                m_NextScrollPosition = new Vector2(0, 1 - (selectedIndex / ((float)m_Selectables.Count - 1)));
            }
        }
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