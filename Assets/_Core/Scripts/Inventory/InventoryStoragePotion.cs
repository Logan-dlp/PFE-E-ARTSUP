using System;
using System.Collections;
using MoonlitMixes.Inputs;
using MoonlitMixes.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MoonlitMixes.Inventory
{
    public class InventoryStoragePotion : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryUI;
        [SerializeField] private ScriptableCloseCanvasEvent _scriptableCloseCanvasEvent;

        private void Start()
        {
            _inventoryUI.SetActive(false);
        }

        private void OnEnable()
        {
            _scriptableCloseCanvasEvent.CloseCanvasAction += CloseInventory;
        }

        private void OnDisable()
        {
            _scriptableCloseCanvasEvent.CloseCanvasAction -= CloseInventory;
        }
    
        public void OpenInventory()
        {
            _inventoryUI.SetActive(true);
            StartCoroutine(FirstSelected());
        }

        public void CloseInventory()
        {
            _inventoryUI.SetActive(false);
            FindFirstObjectByType<PlayerInteraction>().QuitInteraction();
        }

        private IEnumerator FirstSelected()
        {
            yield return new WaitForEndOfFrame();
            //FindFirstObjectByType<EventSystem>().firstSelectedGameObject = FindFirstObjectByType<InventoryUI>()._slots[0].GetComponent<Button>().gameObject;
            FindFirstObjectByType<InventoryUI>()._slots[0].GetComponent<Button>().Select();
        }
    }
}