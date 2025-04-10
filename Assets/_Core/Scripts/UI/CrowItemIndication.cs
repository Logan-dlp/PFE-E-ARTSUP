using System.Collections;
using MoonlitMixes.Events;
using MoonlitMixes.Item;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    public class CrowItemIndication : MonoBehaviour
    {
        [SerializeField] private ScriptableItemUsageEvent _scriptableItemUsageEvent;
        [SerializeField] private ScriptableEvent _scriptableIndicationEvent;
        [SerializeField] private ScriptableEvent _scriptableBubbleEvent;
        [SerializeField] private Animator _animatorImage;
        [SerializeField] private Animator _animatorBubble;
        [SerializeField] private RectTransform _rectTransformBubble;
        [SerializeField] private Image _imageAction;
        [SerializeField] private Sprite _baseSprite;

        private Vector3 _rectTransformBubblePosition;
        private Vector2 _rectTransformBubbleSize;
        private bool _isInAnim;
        private bool _hasAnotherItem;

        private void OnEnable()
        {
            _scriptableItemUsageEvent.OnItemUsageEvent += OpenUIAnim;
            _scriptableIndicationEvent.OnEvent += CloseUIAnim;
            _scriptableBubbleEvent.OnEvent += DesactivateUI;
        }

        private void OnDisable()
        {
            _scriptableItemUsageEvent.OnItemUsageEvent -= OpenUIAnim;
            _scriptableIndicationEvent.OnEvent -= CloseUIAnim;
            _scriptableBubbleEvent.OnEvent -= DesactivateUI;
        }

        private void Awake()
        {
            _rectTransformBubblePosition = _rectTransformBubble.localPosition;
            _rectTransformBubbleSize = _rectTransformBubble.sizeDelta;
        }

        private void OpenUIAnim(ItemUsage itemUsage)
        {
            if (_isInAnim)
            {
                _hasAnotherItem = true;
                ChangeIndicationWithoutDelay(itemUsage);
                StopCoroutine(ChangeIndicationWithDelay(itemUsage));
            }
            else
            {
                _animatorBubble.gameObject.SetActive(true);
                _isInAnim = true;
                StartCoroutine(ChangeIndicationWithDelay(itemUsage));
            }
        }

        private IEnumerator ChangeIndicationWithDelay(ItemUsage itemUsage)
        {
            yield return new WaitForSeconds(1);
            Debug.Log("Delay");
            _animatorImage.gameObject.SetActive(true);
            _animatorImage.SetTrigger(itemUsage.ToString());
        }

        private void ChangeIndicationWithoutDelay(ItemUsage itemUsage)
        {
            Debug.Log("Reload");
            _animatorBubble.SetTrigger("Reload");
            _animatorImage.SetTrigger("Reload");
            _animatorImage.SetTrigger(itemUsage.ToString());
        }
        

        private void CloseUIAnim()
        {
            if(_hasAnotherItem)
            {
                _hasAnotherItem = false;
                return;
            }

            _isInAnim = false;
            _imageAction.sprite = _baseSprite;
            _animatorImage.gameObject.SetActive(false);
            _animatorBubble.SetTrigger("Close");
        }

        private void DesactivateUI()
        {
            

            _animatorBubble.gameObject.SetActive(false);
            
            _rectTransformBubble.localPosition = _rectTransformBubblePosition; 
            _rectTransformBubble.sizeDelta = _rectTransformBubbleSize;
        }
    }
}
