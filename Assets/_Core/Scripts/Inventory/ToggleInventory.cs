using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleInventory : MonoBehaviour
{
    [SerializeField] private GameObject _canvaInventory;
    private bool isActive = false;

    private void Start()
    {
        if (_canvaInventory != null)
        {
            _canvaInventory.SetActive(isActive);
        }
    }

    public void Toggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isActive = !isActive;
            _canvaInventory.SetActive(isActive);
        }
    }
}
