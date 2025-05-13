using MoonlitMixes.Datas;
using MoonlitMixes.Inputs;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Inventory
{
    public class ChestInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject _filledItemUI;
        [SerializeField] private Button _filledItemButton;
        [SerializeField] private GameObject _emptyItemUI;
        [SerializeField] private Button _emptyItemButton;
        [SerializeField] private InventoryData _inventory;

        private TriggerButtonUI _triggerZone;

        private void Awake()
        {
            _triggerZone = FindAnyObjectByType<TriggerButtonUI>();
        }

        public void OpenChest()
        {
            if (_triggerZone != null && _triggerZone.isPlayerInTrigger)
            {
                bool hasEmptyItem = _inventory.Items.Count == 0;
                FindFirstObjectByType<InputManager>().SwitchActionMap("UI");

                if(hasEmptyItem)
                {
                    _emptyItemUI.SetActive(true);
                    _emptyItemButton.Select();
                }
                else
                {
                    _filledItemUI.SetActive(true);
                    _filledItemButton.Select();
                }
            }
        }
    }
}