using UnityEngine;
using UnityEngine.UI;
using MoonlitMixes.Shop;

namespace MoonlitMixes.Player.Interaction
{
    public class InteractionButton : MonoBehaviour
    {
        [SerializeField] private Image _buttonImage;
        [SerializeField] private CloseOrOpenShop _closeOrOpenShop;

        private void Start()
        {
            if (_buttonImage != null)
            {
                _buttonImage.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_closeOrOpenShop.HasShopBeenOpened)
            {
                if (_buttonImage != null)
                {
                    _buttonImage.gameObject.SetActive(true);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_buttonImage != null)
            {
                _buttonImage.gameObject.SetActive(false);
            }
        }

        public void DeactivateButtonUI()
        {
            if (_buttonImage != null)
            {
                _buttonImage.gameObject.SetActive(false);
            }
        }
    }
}