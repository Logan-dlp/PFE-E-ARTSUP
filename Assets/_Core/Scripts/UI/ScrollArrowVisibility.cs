using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    public class ScrollArrowVisibility : MonoBehaviour
    {
        [SerializeField] private GameObject _topArrow;
        [SerializeField] private GameObject _bottomArrow;

        private ScrollRect _scrollRect;

        private void Awake()
        {
            _scrollRect = GetComponentInParent<ScrollRect>();
            _scrollRect.verticalScrollbar.onValueChanged.AddListener(UpdateArrowVisibility);

            _topArrow.SetActive(false);
            _bottomArrow.SetActive(false);
        }

        private void UpdateArrowVisibility(float scrollBarValue)
        {
            if (scrollBarValue > .9f)
            {
                _topArrow.SetActive(false);
                _bottomArrow.SetActive(true);
            }
            else if (scrollBarValue < .1f)
            {
                _topArrow.SetActive(true);
                _bottomArrow.SetActive(false);
            }
            else
            {
                _topArrow.SetActive(true);
                _bottomArrow.SetActive(true);
            }
        }
    }
}