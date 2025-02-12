using System.Collections;
using MoonlitMixes.Inventory;
using MoonlitMixes.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryStoragePotion : MonoBehaviour
{
    [SerializeField] private Canvas _inventoryUI;
    [SerializeField] private ScriptableCloseCanvasEvent _scriptableCloseCanvasEvent;

    private void Start()
    {
        CloseInventory();
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
        _inventoryUI.gameObject.SetActive(true);
        StartCoroutine(FirstSelected());
    }

    public void CloseInventory()
    {
        _inventoryUI.gameObject.SetActive(false);
        FindFirstObjectByType<PlayerInteraction>().QuitInteraction();
    }

    private IEnumerator FirstSelected()
    {
        yield return new WaitForEndOfFrame();
        FindFirstObjectByType<InventoryUI>()._slots[0].GetComponent<Button>().Select();
    }
}
