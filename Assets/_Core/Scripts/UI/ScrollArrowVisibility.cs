using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScrollArrowVisibility : MonoBehaviour
{
    [SerializeField] private GameObject _topArrow;
    [SerializeField] private GameObject _bottomArrow;
    private ScrollRect _scrollRect;

    private void Start()
    {
        _scrollRect = GetComponentInParent<ScrollRect>();
        UpdateArrowVisibility();
    }

    private void Update()
    {
        UpdateArrowVisibility();
    }

    private void UpdateArrowVisibility()
    {
        float normalizedPosition = _scrollRect.verticalNormalizedPosition;

        if (normalizedPosition >= 0.9f)
        {
            _topArrow.SetActive(false);
            _bottomArrow.SetActive(true);
        }
        else if (normalizedPosition <= 0.2f)
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