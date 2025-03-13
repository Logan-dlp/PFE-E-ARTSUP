using MoonlitMixes.AI.PNJ;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private CloseOrOpenShop _closeOrOpenShop;

    private void Start()
    {
        if (buttonImage != null)
        {
            buttonImage.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_closeOrOpenShop.HasShopBeenOpened)
        {
            if (buttonImage != null)
            {
                buttonImage.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (buttonImage != null)
        {
            buttonImage.gameObject.SetActive(false);
        }
    }

    public void DeactivateButtonUI()
    {
        if (buttonImage != null)
        {
            buttonImage.gameObject.SetActive(false);
        }
    }
}