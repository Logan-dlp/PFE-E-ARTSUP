using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    public class ScrollNavigationController : MonoBehaviour
    {
        [SerializeField] private float _scrollSpeed = 1f;

        private ScrollRect _scrollRect;
        private List<Selectable> _selectables;
        private int _currentIndex = 0;
        private bool _isJoystickMoved = false;

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        private void Start()
        {
            PopulateSelectables();
        }

        public void Navigate(InputAction.CallbackContext context)
        {
            if (_isJoystickMoved || !context.performed) return;

            Vector2 direction = context.ReadValue<Vector2>();

            if (direction.y > 0)
            {
                Move(-1);
            }
            else if (direction.y < 0)
            {
                Move(1);
            }

            _isJoystickMoved = true;
        }

        private void PopulateSelectables()
        {
            _selectables = new List<Selectable>();

            foreach (Transform child in _scrollRect.content)
            {
                Selectable selectable = child.GetComponent<Selectable>();
                if (selectable != null)
                {
                    _selectables.Add(selectable);
                }
            }

            if (_selectables.Count > 0)
            {
                EventSystem.current.SetSelectedGameObject(_selectables[0].gameObject);
                ScrollToSelected();
            }
        }

        private void Move(int step)
        {
            int nextIndex = Mathf.Clamp(_currentIndex + step, 0, _selectables.Count - 1);

            if (nextIndex == _currentIndex) return;

            _currentIndex = nextIndex;
            var selectable = _selectables[_currentIndex];
            EventSystem.current.SetSelectedGameObject(selectable.gameObject);

            ScrollToSelected();
        }

        private void ScrollToSelected()
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;

            if (selected == null || _scrollRect == null) return;

            RectTransform selectedRect = selected.GetComponent<RectTransform>();
            RectTransform content = _scrollRect.content;
            RectTransform viewport = _scrollRect.viewport;

            if (selectedRect == null || content == null || viewport == null) return;

            Vector2 localPosition = (Vector2)content.InverseTransformPoint(selectedRect.position);
            Vector2 viewportLocalPosition = (Vector2)content.InverseTransformPoint(viewport.position);

            float difference = localPosition.y - viewportLocalPosition.y;
        }
    }
}
