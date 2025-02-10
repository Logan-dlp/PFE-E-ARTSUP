using MoonlitMixes.Inventory;
using MoonlitMixes.Item;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseTools : MonoBehaviour
{
    [SerializeField] private InventoryUI _inventory;
    [SerializeField] private int _rochetsCasses = 0;

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
            if (CanUseHand())
            {
                UseHand();
                return;
            }

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
                if (itemList.Items.Count > 0)
                {
                    ItemData itemToAdd = itemList.Items[0];

                    if (_inventory != null)
                    {
                        _inventory.AddItem(itemToAdd);
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
                RockHealth rockHealth = hit.collider.GetComponent<RockHealth>();
                if (rockHealth != null && rockHealth.TakeDamage())
                {
                    if (itemList.Items.Count >= 2)
                    {
                        float chance = GetPreciousStoneChance(_rochetsCasses);
                        ItemData itemToAdd;
                        float randomValue = Random.value;

                        if (randomValue < chance)
                        {
                            itemToAdd = itemList.Items[1];
                        }
                        else
                        {
                            itemToAdd = itemList.Items[0];
                        }

                        _inventory?.AddItem(itemToAdd);

                        _rochetsCasses++;
                        if (_rochetsCasses >= 3)
                        {
                            _rochetsCasses = 0;
                        }
                    }
                }
            }
        }
    }

    private float GetPreciousStoneChance(int rockMined)
    {
        switch (rockMined)
        {
            case 0: return 1f;   // Premier rochet -> 100% de pierre précieuse
            case 1: return 0.4f; // Deuxième rochet -> 40% de chance
            case 2: return 0.1f; // Troisième rochet -> 10% de chance
            default: return 1f;  // Reset après 3 rochets -> Retour à 100%
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
                if (itemList.Items.Count > 0)
                {
                    ItemData itemToAdd = itemList.Items[0];

                    if (_inventory != null)
                    {
                        _inventory.AddItem(itemToAdd);
                    }
                }
                Destroy(hit.collider.gameObject);
            }
        }
    }

    private bool CanUseHand()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            ItemListData itemList = hit.collider.GetComponent<ItemListSource>()?.GetItemList();
            return itemList != null && itemList.ToolType == ToolType.Hand;
        }
        return false;
    }

    private void UseHand()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            ItemListData itemList = hit.collider.GetComponent<ItemListSource>()?.GetItemList();

            if (itemList != null && itemList.ToolType == ToolType.Hand)
            {
                if (itemList.Items.Count > 0)
                {
                    ItemData itemToAdd = itemList.Items[0];

                    if (_inventory != null)
                    {
                        _inventory.AddItem(itemToAdd);
                    }
                }
                Destroy(hit.collider.gameObject);
            }
        }
    }
}