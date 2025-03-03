using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;

    private void Start()
    {
        if (buttonImage != null)
        {
            buttonImage.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (buttonImage != null)
        {
            buttonImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (buttonImage != null)
        {
            buttonImage.gameObject.SetActive(false);
        }
    }
}