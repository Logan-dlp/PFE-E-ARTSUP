using UnityEngine;

public class TriggerButtonUI : MonoBehaviour
{
    [SerializeField] private GameObject _uiButton;
    public bool isPlayerInTrigger { get; private set; } = false;

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
            isPlayerInTrigger = true;
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
            isPlayerInTrigger = false;
            if (_uiButton != null)
            {
                _uiButton.SetActive(false);
            }
        }
    }
}