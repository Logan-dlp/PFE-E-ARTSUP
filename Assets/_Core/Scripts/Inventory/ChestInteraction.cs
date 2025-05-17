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

        public void OpenChest()
        {
            bool hasEmptyItem = _inventory.Items.Count == 0;
            
            if (hasEmptyItem)
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