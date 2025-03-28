using UnityEngine;

public class TriggerButtonUI : MonoBehaviour
{
    [SerializeField] private GameObject _uiButton;

    private void Start()
    {
        if (_uiButton != null)
        {
            _uiButton.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_uiButton != null)
            {
                _uiButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_uiButton != null)
            {
                _uiButton.SetActive(false);
            }
        }
    }
}