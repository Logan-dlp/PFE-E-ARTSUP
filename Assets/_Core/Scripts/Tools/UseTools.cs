using MoonlitMixes.AI;
using MoonlitMixes.Animation;
using MoonlitMixes.ExplorationTools;
using MoonlitMixes.Inventory;
using MoonlitMixes.Item;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseTools : MonoBehaviour
{
    public static event System.Action OnUsedMachete;
    public static event System.Action OnUsedPickaxe;
    public static event System.Action OnUsedSepter;
    public static event System.Action OnUsedSepterSwing;
    public static event System.Action OnUsedHand;

    [SerializeField] private InventoryUI _inventory;
    [SerializeField] private float _attackDistance;
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _attackForce;
    [SerializeField] private LayerMask _layerHitable;
    [SerializeField] private Vector3 _raycastOffset;
    [SerializeField] private float _septerInvokeDelay = 0.5f;

    private int _brokenRock = 0;
    private RouletteSelectionTools _rouletteSelection;
    private ToolType _currentTool;
    public ToolType CurrentTool
    {
        get => _currentTool;
    }

    private AnimationExplorationManager _animationExplorationManager;

    private void Awake()
    {
        _animationExplorationManager = GetComponent<AnimationExplorationManager>();
        _rouletteSelection = FindFirstObjectByType<RouletteSelectionTools>();
        if (_rouletteSelection == null)
        {
            Debug.LogError("❌ Aucun RouletteSelectionTools trouvé dans la scène !");
        }
    }

    public void UseTool(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (_rouletteSelection.ToolGameObjects.Count == 0) return;

            _currentTool = _rouletteSelection.CurrentToolType;

            switch (_currentTool)
            {
                case ToolType.Machete:
                    UseMachete();
                    break;
                case ToolType.Pickaxe:
                    UsePickaxe();
                    break;
                case ToolType.Staff:
                    UseSepter();
                    break;
            }
        }
    }

    public void CollectItems(ItemListSource itemListSource)
    {
        ItemListData itemList = itemListSource?.GetItemList();
        if (itemList != null)
        {
            if (itemList.Items.Count > 0)
            {
                ItemData item = itemList.Items[0];

                if (_inventory != null)
                {
                    _inventory.AddItem(item);
                }
            }
        }
    }

    private void UseMachete()
    {
        if (Physics.Raycast(transform.position + _raycastOffset, transform.forward, out RaycastHit hit, 2f, _layerHitable))
        {
            var itemSource = hit.collider.GetComponent<ItemListSource>();
            var itemList = itemSource?.GetItemList();

            if (itemList != null && itemList.ToolType == ToolType.Machete)
            {
                TreeHealth treeHealth = hit.collider.GetComponent<TreeHealth>();

                if (treeHealth == null)
                {
                    Debug.LogWarning("Aucun TreeHealth trouvé sur cet objet.");
                    return;
                }

                if (!treeHealth.CanChop())
                {
                    Debug.Log("Cet arbre a déjà été coupé deux fois.");
                    return;
                }

                treeHealth.Chop();

                if (itemList.Items.Count > 0)
                {
                    ItemData itemToAdd = itemList.Items[0];

                    if (_inventory != null)
                    {
                        _inventory.AddItem(itemToAdd);
                        _animationExplorationManager.UseMachete();
                        OnUsedMachete?.Invoke();
                    }
                }
            }
        }
    }

    private void UsePickaxe()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + _raycastOffset, transform.forward, out hit, 2f, _layerHitable))
        {
            ItemListData itemList = hit.collider.GetComponent<ItemListSource>()?.GetItemList();

            if (itemList != null && itemList.ToolType == ToolType.Pickaxe)
            {
                RockHealth rockHealth = hit.collider.GetComponent<RockHealth>();
                if (rockHealth != null && rockHealth.TakeDamage())
                {
                    if (itemList.Items.Count >= 2)
                    {
                        float chance = GetPreciousStoneChance(_brokenRock);
                        ItemData itemToAdd;
                        float randomValue = Random.value;

                        if (randomValue < chance)
                        {
                            itemToAdd = itemList.Items[1];
                            _inventory?.AddItem(itemToAdd);
                        }

                        _brokenRock++;

                        if (_brokenRock >= 3)
                        {
                            _brokenRock = 0;
                        }

                        _animationExplorationManager.UsePickaxe();
                        OnUsedPickaxe?.Invoke();
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
        _animationExplorationManager.UseStaff();
        OnUsedSepterSwing?.Invoke();
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position + _raycastOffset, transform.forward, out hit, _attackDistance))
        {
            if (hit.transform.TryGetComponent(out Monster monster))
            {
                monster.Damage(gameObject, _attackDamage, transform.forward, _attackForce);
                StartCoroutine(DelayedSepterHitInvoke());
            }
        }
    }

    private IEnumerator DelayedSepterHitInvoke()
    {
        yield return new WaitForSeconds(_septerInvokeDelay);
        OnUsedSepter?.Invoke();
    }

    public bool CanUseHand()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + _raycastOffset, transform.forward, out hit, 2f, _layerHitable))
        {
            ItemListData itemList = hit.collider.GetComponent<ItemListSource>()?.GetItemList();
            return itemList != null && itemList.ToolType == ToolType.Hand;
        }
        return false;
    }

    public void UseHand()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + _raycastOffset, transform.forward, out hit, 2f, _layerHitable))
        {
            ItemListData itemList = hit.collider.GetComponent<ItemListSource>()?.GetItemList();

            if (itemList != null && itemList.ToolType == ToolType.Hand)
            {
                if (itemList.Items.Count > 0)
                {
                    ItemData itemToAdd = itemList.Items[0];

                    if (_inventory != null)
                    {
                        Debug.Log("Interact");
                        _inventory.AddItem(itemToAdd);
                        _animationExplorationManager.Interaction();
                    }
                }
                Destroy(hit.collider.gameObject);
            }
        }
    }

    public void UseHand(ItemListSource itemListSource)
    {
        if (itemListSource == null)
            return;
        
        if (itemListSource.GetItemList() != null && itemListSource.GetItemList().Items.Count > 0)
        {
            OnUsedHand?.Invoke();

            if (_inventory != null)
            {
                _inventory.AddItem(itemListSource.GetItemList().Items[0]);
                _animationExplorationManager.Interaction();
            }
            Destroy(itemListSource.gameObject);
        }
    }
}