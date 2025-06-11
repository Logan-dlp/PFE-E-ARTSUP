using System.Collections;
using System.Collections.Generic;
using MoonlitMixes.Datas;
using MoonlitMixes.Inputs;
using UnityEngine;
using UnityEngine.EventSystems;
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
            IEnumerator ActiveOnClick(Button button)
            {
                button.interactable = false;
                yield return new WaitForEndOfFrame();
                button.interactable = true;
            }
            
            if (_inventory.Items.Count == 0)
            {
                InputManager.Instance.SwitchActionMap("UI");
                _emptyItemUI.SetActive(true);
                StartCoroutine(ActiveOnClick(_emptyItemButton));
                EventSystem.current.SetSelectedGameObject(_emptyItemButton.gameObject);
            }
            else
            {
                InputManager.Instance.SwitchActionMap("UI");
                _filledItemUI.SetActive(true);
                StartCoroutine(ActiveOnClick(_filledItemButton));
                EventSystem.current.SetSelectedGameObject(_filledItemButton.gameObject);
            }
        }
    }
}