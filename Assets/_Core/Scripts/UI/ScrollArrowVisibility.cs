using UnityEngine;
using UnityEngine.UI;

public class ScrollArrowVisibility : MonoBehaviour
{
    [SerializeField] private GameObject topArrow;
    [SerializeField] private GameObject bottomArrow;
    private ScrollRect scrollRect;

    private void Start()
    {
        scrollRect = GetComponentInParent<ScrollRect>();
        UpdateArrowVisibility();
    }

    private void Update()
    {
        UpdateArrowVisibility();
    }

    private void UpdateArrowVisibility()
    {
        float normalizedPosition = scrollRect.verticalNormalizedPosition;

        if (normalizedPosition >= 0.9f)
        {
            topArrow.SetActive(false);
            bottomArrow.SetActive(true);
        }
        else if (normalizedPosition <= 0.2f)
        {
            topArrow.SetActive(true);
            bottomArrow.SetActive(false);
        }
        else
        {
            topArrow.SetActive(true);
            bottomArrow.SetActive(true);
        }
    }
}