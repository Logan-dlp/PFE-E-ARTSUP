using DG.Tweening;
using MoonlitMixes.Events;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace MoonlitMixes.Rendering
{
    public class SwitchGlobalVolume : MonoBehaviour
    {
        [SerializeField] private Volume _volumeStressFree;
        [SerializeField] private Volume _volumeStressHigh;
        [SerializeField] private Image _wolfStressFaceImage;
        [SerializeField] private Sprite _wolfStressFreeSprite;
        [SerializeField] private Sprite _wolfStressHighSprite;
        [SerializeField] private float _transitionTime;
        [SerializeField] private ScriptableEvent _scriptableEvent;

        private void OnEnable()
        {
            _scriptableEvent.OnEvent += StressFree;
        }

        private void OnDisable()
        {
            _scriptableEvent.OnEvent -= StressFree;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "Player")
            {
                HighStress();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.transform.tag == "Player")
            {
                StressFree();
            }
        }

        private void HighStress()
        {
            _wolfStressFaceImage.sprite = _wolfStressHighSprite;
            DOTween.To(() => _volumeStressFree.weight, x => _volumeStressFree.weight = x, 0f, _transitionTime);
            DOTween.To(() => _volumeStressHigh.weight, x => _volumeStressHigh.weight = x, 1f, _transitionTime);
        }

        private void StressFree()
        {
            _wolfStressFaceImage.sprite = _wolfStressFreeSprite;
            DOTween.To(() => _volumeStressFree.weight, x => _volumeStressFree.weight = x, 1f, _transitionTime);
            DOTween.To(() => _volumeStressHigh.weight, x => _volumeStressHigh.weight = x, 0f, _transitionTime);
        }
    }
}
