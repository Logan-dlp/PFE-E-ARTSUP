using MoonlitMixes.Animation;
using MoonlitMixes.CookingMachine;
using MoonlitMixes.Datas;
using MoonlitMixes.Inputs;
using MoonlitMixes.Inventory;
using MoonlitMixes.Item;
using MoonlitMixes.Potion;
using MoonlitMixes.Scene;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        public ItemData ItemInHand { get; set; }
        public PlayerHoldItem PlayerHoldItem { get; private set; }
        public PlayerInput CurrentPlayerInput { get; private set; }

        [SerializeField] private float _interactionDistance;
        [SerializeField] private LayerMask _layerHitable;
        [SerializeField] private string _actionMapPlayer;
        [SerializeField] private string _actionMapQTE;
        [SerializeField] private string _actionMapWaitingTable;
        [SerializeField] private string _actionMapUI;

        private InventoryStoragePotion _inventoryStoragePotion;
        private ACookingMachine _currentCookingMachine;
        private CauldronRecipeChecker _currentCauldron;
        private Animator _animator;
        private AnimationPotionManager _animationPotionManager;
        private Trashcan _currentTrashcan;

        private void Awake()
        {
            CurrentPlayerInput = GetComponent<PlayerInput>();
            PlayerHoldItem = GetComponent<PlayerHoldItem>();
            _animator = GetComponent<Animator>();
            _animationPotionManager = GetComponent<AnimationPotionManager>();
            _inventoryStoragePotion = FindFirstObjectByType<InventoryStoragePotion>();
        }

        private void Update()
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, out RaycastHit hit, _interactionDistance, _layerHitable))
            {
                if (ItemInHand != null)
                {
                    if (hit.transform.TryGetComponent(out ACookingMachine cookingMachine) && cookingMachine.TransformType == ItemInHand.Usage)
                    {
                        if (_currentCookingMachine != cookingMachine)
                        {
                            SetNewCookingMachine(cookingMachine);
                        }
                    }
                    else if (hit.transform.TryGetComponent(out CauldronRecipeChecker cauldron))
                    {
                        if (_currentCauldron != cauldron)
                        {
                            SetNewCauldron(cauldron);
                        }
                    }
                    else if (hit.transform.TryGetComponent(out WaitingTable waitingTable))
                    {
                        ResetInteractionTargets();
                    }
                }
            }
            else if (_currentCookingMachine != null || _currentCauldron != null)
            {
                ResetInteractionTargets();
            }
        }

        private void SetNewCauldron(CauldronRecipeChecker newCauldron)
        {
            if (_currentCauldron != null)
            {
                _currentCauldron.ToggleShowInteractivity();
            }
            newCauldron.ToggleShowInteractivity();
            _currentCauldron = newCauldron;
            _currentCookingMachine = null;
        }

        private void SetNewCookingMachine(ACookingMachine newCookingMachine)
        {
            if (_currentCookingMachine != null)
            {
                _currentCookingMachine.ToggleShowInteractivity();
            }
            newCookingMachine.ToggleShowInteractivity();
            _currentCookingMachine = newCookingMachine;
            _currentCauldron = null;
        }

        public void SetCurrentTrashcan(Trashcan trashcan)
        {
            _currentTrashcan = trashcan;
        }

        public void ClearCurrentTrashcan(Trashcan trashcan)
        {
            if (_currentTrashcan == trashcan)
            {
                _currentTrashcan = null;
            }
        }

        private void ResetInteractionTargets()
        {
            if (_currentCauldron != null) _currentCauldron.ToggleShowInteractivity();
            _currentCauldron = null;
            if (_currentCookingMachine != null) _currentCookingMachine.ToggleShowInteractivity();
            _currentCookingMachine = null;
            _currentTrashcan = null;
        }

        public void ReturnItemToCellar()
        {
            if (_inventoryStoragePotion != null && PlayerHoldItem.TryReturnItemToCellar(_inventoryStoragePotion.CellarInventory))
            {
                Debug.Log("Item successfully returned to cellar.");
                ItemInHand = null;
                _animationPotionManager.QuitInteractWithItem();
            }
            else
            {
                Debug.LogWarning("Failed to return item to cellar.");
            }
        }

        public void Interact(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                if (ItemInHand != null)
                {
                    if (Physics.Raycast(transform.position, transform.forward + new Vector3(0, 1, 0), out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        if (hit.transform.TryGetComponent(out InventoryStoragePotion inventory))
                        {
                            bool success = PlayerHoldItem.TryReturnItemToCellar(_inventoryStoragePotion.CellarInventory);
                            if (success)
                            {
                                ItemInHand = null;
                                _animationPotionManager.QuitInteractWithItem();
                                Debug.Log("Item returned to cellar.");
                            }
                            else
                            {
                                Debug.LogWarning("Failed to return item to cellar.");
                            }
                            return;
                        }

                        if (hit.transform.TryGetComponent(out WaitingTable waitingTable) && waitingTable.CheckAvailablePlace())
                        {
                            waitingTable.PlaceItem(PlayerHoldItem.ItemHold);
                            PlayerHoldItem.RemoveItem();
                            ItemInHand = null;

                            _animator.SetTrigger("Put");

                            if (PlayerHoldItem.ItemHold == null)
                                _animationPotionManager.QuitInteractWithoutItem();
                            else
                                _animationPotionManager.QuitInteractWithItem();

                            return;
                        }

                        if (_currentCookingMachine != null && ItemInHand.Usage == _currentCookingMachine.TransformType)
                        {
                            InputManager.Instance.SwitchActionMap(_actionMapQTE);
                            _animationPotionManager.QuitInteractWithoutItem();

                            if (ItemInHand.Usage == ItemUsage.Cut)
                                _animationPotionManager.InteractCut();
                            else if (ItemInHand.Usage == ItemUsage.Crush)
                                _animationPotionManager.InteractCrush();

                            _currentCookingMachine.ConvertItem(ItemInHand, this);
                            return;
                        }
                        else if (_currentCauldron != null && _currentCauldron.GetComponent<CauldronTimer>().CanAction)
                        {
                            if (ItemInHand.Usage == ItemUsage.Whole && _currentCauldron.NeedItem)
                            {
                                _currentCauldron.AddIngredient(ItemInHand);
                                PlayerHoldItem.RemoveItem();
                                _animationPotionManager.InteractCauldronWithoutStir();
                                return;
                            }
                        }
                    }

                    if (_currentTrashcan != null)
                    {
                        _currentTrashcan.DiscardItem();
                        PlayerHoldItem.RemoveItem();
                        _animationPotionManager.TrashItem();
                        return;
                    }
                }
                else
                {
                    if (Physics.Raycast(transform.position, transform.forward + new Vector3(0, 1, 0), out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        Debug.Log("");
                        if (hit.transform.TryGetComponent(out InventoryStoragePotion inventory))
                        {
                            InputManager.Instance.SwitchActionMap(_actionMapUI);
                            inventory.OpenInventory();
                            _animationPotionManager.OpenInventory();
                            return;
                        }
                        else if (hit.transform.TryGetComponent(out WaitingTable waitingTable))
                        {
                            InputManager.Instance.SwitchActionMap(_actionMapWaitingTable);
                            waitingTable.StartHighlight();
                            return;
                        }
                        else if (hit.transform.TryGetComponent(out DoorSceneChange doorSceneChange))
                        {
                            doorSceneChange.OpenCanvas();
                            return;
                        }
                    }
                }
            }
        }

        public void QuitInteraction()
        {
            InputManager.Instance.SwitchActionMap(_actionMapPlayer);

            _animationPotionManager.CloseInventory();
            _animationPotionManager.FinishedInteractCut();
            _animationPotionManager.FinishedInteractCrush();
            _animationPotionManager.FinishedInteractStir();
        }
    }
}