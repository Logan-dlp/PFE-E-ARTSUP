using UnityEngine;
using UnityEngine.InputSystem;
using MoonlitMixes.CookingMachine;
using MoonlitMixes.Inputs;
using MoonlitMixes.Inventory;
using MoonlitMixes.Item;
using MoonlitMixes.Potion;

namespace MoonlitMixes.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance;
        [SerializeField] private LayerMask _layerHitable;
        [SerializeField] private string _actionMapPlayer;
        [SerializeField] private string _actionMapQTE;
        [SerializeField] private string _actionMapWaitingTable;
        [SerializeField] private string _actionMapUI;

        private ACookingMachine _currentCookingMachine;
        private CauldronRecipeChecker _currentCauldron;
        private Animator _animator;
        private PlayerMovement _playerMovement;
        private Trashcan _currentTrashcan;

        public ItemData ItemInHand { get; set; }
        public PlayerHoldItem PlayerHoldItem { get; private set; }

        private void Awake()
        {
            PlayerHoldItem = GetComponent<PlayerHoldItem>();
            _animator = GetComponent<Animator>();
            _playerMovement = GetComponent<PlayerMovement>();
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

                    if (hit.transform.TryGetComponent(out Trashcan trashcan))
                    {
                        trashcan.AnimMouth(true);
                        _currentTrashcan = trashcan;
                    }
                }
            }
            else if (_currentCookingMachine != null || _currentCauldron != null)
            {
                ResetInteractionTargets();
            }
            else if (_currentTrashcan != null)
            {
                _currentTrashcan.AnimMouth(false);
                _currentTrashcan = null;
            }
        }

        private void SetNewCauldron(CauldronRecipeChecker newCauldron)
        {
            if (_currentCauldron != null)
            {
                _currentCauldron.TogleShowInteractivity();
            }
            newCauldron.TogleShowInteractivity();
            _currentCauldron = newCauldron;
            _currentCookingMachine = null;
        }

        private void SetNewCookingMachine(ACookingMachine newCookingMachine)
        {
            if (_currentCookingMachine != null)
            {
                _currentCookingMachine.TogleShowInteractivity();
            }
            newCookingMachine.TogleShowInteractivity();
            _currentCookingMachine = newCookingMachine;
            _currentCauldron = null;
        }

        private void ResetInteractionTargets()
        {
            if (_currentCauldron != null) _currentCauldron.TogleShowInteractivity();
            _currentCauldron = null;
            if (_currentCookingMachine != null) _currentCookingMachine.TogleShowInteractivity();
            _currentCookingMachine = null;
        }

        public void Interact(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                if (ItemInHand != null)
                {
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        if (hit.transform.TryGetComponent(out WaitingTable waitingTable) && waitingTable.CheckAvailablePlace())
                        {
                            waitingTable.PlaceItem(PlayerHoldItem.ItemHold);
                            PlayerHoldItem.RemoveItem();
                            ItemInHand = null;

                            _animator.SetBool("isPut", true);
                            Invoke(nameof(ResetPutAnimation), 0.5f);
                            if (PlayerHoldItem.ItemHold == null)
                            {
                                _playerMovement.SetPerformingActionIdle(true);
                            }
                            else
                            {
                                _playerMovement.SetPerformingActionHolding(true);
                            }
                        }
                        else if (hit.transform.TryGetComponent(out Trashcan trashcan))
                        {
                            trashcan.DiscardItem();
                            PlayerHoldItem.RemoveItem();
                            _animator.SetBool("isThrow", true);
                            Invoke(nameof(ResetThrowAnimation), 0.5f);
                            _playerMovement.SetPerformingActionHolding(false);
                        }
                    }

                    if (_currentCookingMachine != null)
                    {
                        InputManager.Instance.SwitchActionMap(_actionMapQTE);

                        _playerMovement.SetPerformingActionHolding(false);
                        _playerMovement.SetPerformingActionIdle(false);

                        if (ItemInHand.Usage == ItemUsage.Cut)
                        {
                            _playerMovement.InteractCut();
                        }
                        else if (ItemInHand.Usage == ItemUsage.Crush)
                        {
                            _playerMovement.InteractCrush();
                        }

                        _currentCookingMachine.ConvertItem(ItemInHand, this);
                    }
                    else if (_currentCauldron != null && _currentCauldron.GetComponent<CauldronTimer>().CanAction)
                    {
                        if (ItemInHand.Usage == ItemUsage.Whole && _currentCauldron.NeedItem)
                        {
                            _currentCauldron.AddIngredient(ItemInHand);
                            PlayerHoldItem.RemoveItem();

                            _playerMovement.SetPerformingActionIdle(true);
                        }
                    }
                }
                else
                {
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        if (hit.transform.TryGetComponent(out InventoryStoragePotion inventory))
                        {
                            InputManager.Instance.SwitchActionMap(_actionMapUI);
                            inventory.OpenInventory();
                            _playerMovement.OpenInventory();
                            _playerMovement.SetPerformingActionIdle(false);
                        }
                        else if (hit.transform.TryGetComponent(out WaitingTable waitingTable))
                        {
                            InputManager.Instance.SwitchActionMap(_actionMapWaitingTable);
                            waitingTable.StartHighlight();
                        }
                        else if (hit.transform.TryGetComponent(out CauldronRecipeChecker cauldron) && cauldron.GetComponent<CauldronTimer>().CanAction)
                        {
                            if (!cauldron.NeedMix) return;
                            
                            InputManager.Instance.SwitchActionMap(_actionMapQTE);
                            cauldron.Mix(this);

                            _playerMovement.InteractMix();
                        }
                    }
                }
            }
        }

        private void ResetPutAnimation()
        {
            _animator.SetBool("isPut", false);
            _animator.SetBool("isHoldingIdle", true);
            _playerMovement.SetPerformingActionHolding(false);
        }

        private void ResetThrowAnimation()
        {
            _animator.SetBool("isThrow", false);
            _animator.SetBool("isHoldingIdle", true);
            _playerMovement.SetPerformingActionHolding(false);
        }

        public void QuitInteraction()
        {
            InputManager.Instance.SwitchActionMap(_actionMapPlayer);
            _playerMovement.CloseInventory();

            _playerMovement.SetPerformingActionHolding(false);
            _playerMovement.SetPerformingActionIdle(false);

            Invoke(nameof(UpdatePlayerMovementState), 0.5f);

            _playerMovement.FinishedInteractCut();
            _playerMovement.FinishedInteractCrush();
            _playerMovement.FinishedInteractMix();
        }

        private void UpdatePlayerMovementState()
        {
            if (PlayerHoldItem.ItemHold == null)
            {
                _playerMovement.SetPerformingActionIdle(true);
            }
            else
            {
                _playerMovement.SetPerformingActionHolding(true);
            }
        }
    }
}