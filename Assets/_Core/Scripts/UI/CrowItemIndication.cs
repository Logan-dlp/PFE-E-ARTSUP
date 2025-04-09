using System.Collections;
using MoonlitMixes.Events;
using MoonlitMixes.Item;
using UnityEngine;

namespace MoonlitMixes.UI
{
    public class CrowItemIndication : MonoBehaviour
    {
        [SerializeField] private ScriptableItemUsageEvent _scriptableItemUsageEvent;
        [SerializeField] private ScriptableEvent _scriptableIndicationEvent;
        [SerializeField] private ScriptableEvent _scriptableBubbleEvent;
        [SerializeField] private Animator _animatorImage;
        [SerializeField] private Animator _animatorBubble;

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

        private void OpenUIAnim(ItemUsage itemUsage)
        {
            _animatorBubble.gameObject.SetActive(true);
            StartCoroutine(ChangeIndication(itemUsage));
        }

        private IEnumerator ChangeIndication(ItemUsage itemUsage)
        {
            yield return new WaitForSeconds(1);
            _animatorImage.gameObject.SetActive(true);
            _animatorImage.SetTrigger(itemUsage.ToString());
        }

        private void CloseUIAnim()
        {
            _animatorImage.gameObject.SetActive(false);
            _animatorBubble.SetTrigger("Close");
        }

        private void DesactivateUI()
        {
            _animatorBubble.gameObject.SetActive(false);
        }
    }
}
