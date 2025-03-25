using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MoonlitMixes.UI
{
    public class AutoScroll : MonoBehaviour
    {
        [SerializeField] private RectTransform _viewSortRectTransform;
        [SerializeField] private RectTransform _content;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private float _transitionDuration = 0.2f;
    
        private TransitionHelper _transitionHelper = new TransitionHelper();
    
        private void Start()
        {
            StartCoroutine(FixContentSize());
        }
    
        private void Update()
        {
            if (_transitionHelper.InProgress)
            {
                _transitionHelper.Update();
                _content.transform.position = _transitionHelper.PosCurrent;
            }
        }
    
        public void HandleOnSelectChange(GameObject selectedItem)
        {
            RectTransform itemRect = selectedItem.GetComponent<RectTransform>();
            if (itemRect == null || _scrollRect == null)
                return;
    
            float viewportHeight = _viewSortRectTransform.rect.height;
            float contentHeight = _content.rect.height;
    
            float itemPosY = Mathf.Abs(itemRect.anchoredPosition.y);
            float itemHeight = itemRect.rect.height;
    
            float viewportMin = Mathf.Abs(_content.anchoredPosition.y);
            float viewportMax = viewportMin + viewportHeight;
    
            // Si l'élément est trop bas → SCROLL DOWN
            if (itemPosY + itemHeight > viewportMax)
            {
                float newY = itemPosY + itemHeight - viewportHeight;
                _scrollRect.verticalNormalizedPosition = Mathf.Clamp01(1f - (newY / (contentHeight - viewportHeight)));
            }
            
            // Si l'élément est trop haut → SCROLL UP
            else if (itemPosY < viewportMin)
            {
                if (itemPosY < 0f)
                {
                    _scrollRect.verticalNormalizedPosition = 1f;
                    _content.anchoredPosition = Vector2.zero;
                }
                else
                {
                    float newY = itemPosY;
                    _scrollRect.verticalNormalizedPosition = Mathf.Clamp01(1f - (newY / (contentHeight + viewportHeight)));
                }
            }
    
            // Vérification si l'élément sélectionné se trouve dans la première ligne
            if (itemPosY < itemHeight)
            {
                // Alors la première ligne est entièrement visible
                float firstLineY = viewportMin - (viewportMin % itemHeight);
                _scrollRect.verticalNormalizedPosition = Mathf.Clamp01(1f - (firstLineY / (contentHeight - viewportHeight)));
            }
        }
    
        private IEnumerator FixContentSize()
        {
            yield return new WaitForEndOfFrame();
            if (_content != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
                ContentSizeFitter fitter = _content.GetComponent<ContentSizeFitter>();
                if (fitter != null)
                {
                    fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                }
            }
        }
    
        private class TransitionHelper
        {
            private float _duration = 0f;
            private float _timeElapsed = 0f;
            private float _progress = 0f;
            private bool _inProgress;
            private Vector2 _posCurrent;
            private Vector2 _posFrom;
            private Vector2 _posTo;
    
            public bool InProgress => _inProgress;
            public Vector2 PosCurrent => _posCurrent;
    
            public void Update()
            {
                Tick();
                CalculatePosition();
            }
    
            public void TransitionPositionFromTo(Vector2 posFrom, Vector2 posTo, float duration)
            {
                _posFrom = posFrom;
                _posTo = posTo;
                _duration = duration;
                _timeElapsed = 0f;
                _progress = 0f;
                _inProgress = true;
            }
    
            private void CalculatePosition()
            {
                _posCurrent.y = Mathf.Lerp(_posFrom.y, _posTo.y, _progress);
            }
    
            private void Tick()
            {
                if (!_inProgress) return;
    
                _timeElapsed += Time.deltaTime;
                _progress = _timeElapsed / _duration;
                _progress = Mathf.Clamp01(_progress);
    
                if (_progress >= 1f) _inProgress = false;
            }
        }
    }
}