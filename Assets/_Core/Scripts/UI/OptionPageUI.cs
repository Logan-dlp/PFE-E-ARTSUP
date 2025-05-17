using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.UI
{
    public class OptionPageUI : MonoBehaviour
    {
        [SerializeField] private Sprite _spriteOption;
        [SerializeField] private Sprite _spriteControls;
        [SerializeField] private Sprite _spriteCredits;
        [SerializeField] private Sprite _spriteVolume;
        [SerializeField] private GameObject _volumeOptions;

        private Image _imageOption;

        private void Start()
        {
            _imageOption = GetComponent<Image>();
        }

        public void OpenVolume()
        {
            _imageOption.sprite = _spriteVolume;
            _volumeOptions.SetActive(true);
        }

        public void OpenControls()
        {
            _imageOption.sprite = _spriteControls;
        }
        public void OpenCredits()
        {
            _imageOption.sprite = _spriteCredits;
        }
        public void OpenOption()
        {
            _imageOption.sprite = _spriteOption;
            _volumeOptions.SetActive(false);
        }
    }
}