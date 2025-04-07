using UnityEngine;
using UnityEngine.InputSystem;
using MoonlitMixes.CookingMachine;
using MoonlitMixes.Inputs;
using MoonlitMixes.Inventory;
using MoonlitMixes.Item;
using MoonlitMixes.Potion;
using MoonlitMixes.Animation;

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
        private AnimationPotionManager _animationPotionManager;
        private Trashcan _currentTrashcan;

        public ItemData ItemInHand { get; set; }
        public PlayerHoldItem PlayerHoldItem { get; private set; }

        private void Awake()
        {
            PlayerHoldItem = GetComponent<PlayerHoldItem>();
            _animator = GetComponent<Animator>();
            _animationPotionManager = GetComponent<AnimationPotionManager>();
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
                    if (Physics.Raycast(transform.position, transform.forward + new Vector3(0, 1, 0), out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        if (hit.transform.TryGetComponent(out WaitingTable waitingTable) && waitingTable.CheckAvailablePlace())
                        {
                            waitingTable.PlaceItem(PlayerHoldItem.ItemHold);
                            PlayerHoldItem.RemoveItem();
                            ItemInHand = null;

                            _animator.SetTrigger("Put");
                            
                            if (PlayerHoldItem.ItemHold == null)
                            {
                                _animationPotionManager.QuitInteractWithoutItem();
                            }
                            else
                            {
                                _animationPotionManager.QuitInteractWithItem();
                            }
                        }
                        else if (hit.transform.TryGetComponent(out Trashcan trashcan))
                        {
                            trashcan.DiscardItem();
                            PlayerHoldItem.RemoveItem();
                            _animationPotionManager.TrashItem();
                        }
                    }

                    if (_currentCookingMachine != null && ItemInHand.Usage == _currentCookingMachine.TransformType)
                    {
                        InputManager.Instance.SwitchActionMap(_actionMapQTE);
                        _animationPotionManager.QuitInteractWithoutItem();

                        if (ItemInHand.Usage == ItemUsage.Cut)
                        {
                            _animationPotionManager.InteractCut();
                        }
                        else if (ItemInHand.Usage == ItemUsage.Crush)
                        {
                            _animationPotionManager.InteractCrush();
                        }

                        _currentCookingMachine.ConvertItem(ItemInHand, this);
                    }
                    else if (_currentCauldron != null && _currentCauldron.GetComponent<CauldronTimer>().CanAction)
                    {
                        if (ItemInHand.Usage == ItemUsage.Whole && _currentCauldron.NeedItem)
                        {
                            _currentCauldron.AddIngredient(ItemInHand);
                            PlayerHoldItem.RemoveItem();

                            _animationPotionManager.InteractCauldronWithoutMix();
                        }
                    }
                }
                else
                {
                    if (Physics.Raycast(transform.position, transform.forward  + new Vector3(0, 1, 0), out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        if (hit.transform.TryGetComponent(out InventoryStoragePotion inventory))
                        {
                            InputManager.Instance.SwitchActionMap(_actionMapUI);
                            inventory.OpenInventory();
                            _animationPotionManager.OpenInventory();

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

                            _animationPotionManager.InteractMix();
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
            _animationPotionManager.FinishedInteractMix();
        }
    }
}