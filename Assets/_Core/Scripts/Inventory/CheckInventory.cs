using MoonlitMixes.Datas;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckInventory : MonoBehaviour
{
    [SerializeField] private GameObject _filledItemUI;
    [SerializeField] private GameObject _emptyItemUI;
    [SerializeField] private InventoryData _inventory;
    
    private TriggerButtonUI _triggerZone;

    private void Awake()
    {
        _triggerZone = FindAnyObjectByType<TriggerButtonUI>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && _triggerZone != null && _triggerZone.isPlayerInTrigger)
        {
            bool hasEmptyItem = _inventory.Items.Count == 0;

            _emptyItemUI.SetActive(hasEmptyItem);
            _filledItemUI.SetActive(!hasEmptyItem);
        }
    }
}