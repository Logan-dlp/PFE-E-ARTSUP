using MoonlitMixes.Datas;
using MoonlitMixes.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Inventory
{
    public class InventoryStoragePotion : MonoBehaviour
    {
        [SerializeField] private GameObject _inventoryUI;
        [SerializeField] private ScriptableCloseCanvasEvent _scriptableCloseCanvasEvent;
        [SerializeField] private InventoryData _inventoryData;
        public InventoryData CellarInventory => _inventoryData;

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
        }

        public void CloseInventory()
        {
            if (_inventoryUI.activeInHierarchy)
            {
                _inventoryUI.SetActive(false);
                FindFirstObjectByType<PlayerInteraction>().QuitInteraction();
            }
        }

        private IEnumerator FirstSelected()
        {
            yield return new WaitForEndOfFrame();
            FindFirstObjectByType<InventoryUI>().FirstSelected.GetComponent<Button>().Select();
        }
    }
}