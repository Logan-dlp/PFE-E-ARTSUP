using MoonlitMixes.Inventory;
using MoonlitMixes.Item;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseTools : MonoBehaviour
{
    [SerializeField] private InventoryUI _inventory;
    private RouletteSelectionTools _rouletteSelection;
    private ToolType _currentTool;

    private void Awake()
    {
        _rouletteSelection = FindObjectOfType<RouletteSelectionTools>();
        if (_rouletteSelection == null)
        {
            Debug.LogError("❌ Aucun RouletteSelectionTools trouvé dans la scène !");
        }
    }

    public void UseTool(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _currentTool = _rouletteSelection.GetCurrentToolType();

            switch (_currentTool)
            {
                case ToolType.Machete:
                    UseMachete();
                    break;
                case ToolType.Pickaxe:
                    UsePickaxe();
                    break;
                case ToolType.Septer:
                    UseSepter();
                    break;
            }
        }
    }

    private void UseMachete()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            ItemListData itemList = hit.collider.GetComponent<ItemListSource>()?.GetItemList();

            if (itemList != null && itemList.ToolType == ToolType.Machete)
            {
                Debug.Log("🗡️ Coup de machette !");

                if (itemList.Items.Count > 0)
                {
                    ItemData itemToAdd = itemList.Items[0];

                    if (_inventory != null)
                    {
                        _inventory.AddItem(itemToAdd);
                        Debug.Log($"{itemToAdd.ObjectName} ajouté à l'inventaire !");
                    }
                }
            }
        }
    }

    private void UsePickaxe()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            ItemListData itemList = hit.collider.GetComponent<ItemListSource>()?.GetItemList();

            if (itemList != null && itemList.ToolType == ToolType.Pickaxe)
            {
                Debug.Log("⛏️ Coup de pioche !");

                if (itemList.Items.Count > 0)
                {
                    ItemData itemToAdd = itemList.Items[0];

                    if (_inventory != null)
                    {
                        _inventory.AddItem(itemToAdd);
                        Debug.Log($"{itemToAdd.ObjectName} ajouté à l'inventaire !");
                    }
                }
            }
        }
    }

    private void UseSepter()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            ItemListData itemList = hit.collider.GetComponent<ItemListSource>()?.GetItemList();

            if (itemList != null && itemList.ToolType == ToolType.Septer)
            {
                Debug.Log("🔥 Le player a touché un ennemi !");

                if (itemList.Items.Count > 0)
                {
                    ItemData itemToAdd = itemList.Items[0];

                    if (_inventory != null)
                    {
                        _inventory.AddItem(itemToAdd);
                        Debug.Log($"{itemToAdd.ObjectName} ajouté à l'inventaire !");
                    }
                }
                Destroy(hit.collider.gameObject);
            }
        }
    }
}