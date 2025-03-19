using UnityEngine;
using UnityEngine.InputSystem;
using MoonlitMixes.CookingMachine;
using MoonlitMixes.Item;

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
        private PlayerInput _playerInput;
        private CauldronRecipeChecker _currentCauldron;
        private Animator _animator;

        public ItemData ItemInHand { get; set; }
        public PlayerHoldItem PlayerHoldItem { get; private set;}

        private void Awake()
        {
            PlayerHoldItem = GetComponent<PlayerHoldItem>();
            _playerInput = GetComponent<PlayerInput>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _layerHitable))
            {
                if(ItemInHand != null)
                {
                    if (hit.transform.TryGetComponent(out ACookingMachine cookingMachine) && cookingMachine.TransformType == ItemInHand.Usage)
                    {
                        if (_currentCookingMachine != cookingMachine)
                        {
                            SetNewCookingMachine(cookingMachine);
                        }
                    }
                    else if(hit.transform.TryGetComponent(out CauldronRecipeChecker cauldron))
                    {
                        if (_currentCauldron != cauldron)
                        {
                            SetNewCauldron(cauldron);
                        }
                    }
                    else if(hit.transform.TryGetComponent(out WaitingTable waitingTable))
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
            if(_currentCauldron != null) _currentCauldron.TogleShowInteractivity();
            _currentCauldron = null;
            if(_currentCookingMachine != null) _currentCookingMachine.TogleShowInteractivity();
            _currentCookingMachine = null;
        }

        public void Interact(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                if(ItemInHand != null)
                {
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        if(hit.transform.TryGetComponent(out WaitingTable waitingTable) && waitingTable.CheckAvailablePlace())
                        {
                            waitingTable.PlaceItem(PlayerHoldItem.ItemHold);
                            PlayerHoldItem.RemoveItem();
                            ItemInHand = null;
                            _animator.SetBool("isPut", true);
                            Invoke(nameof(ResetPutAnimation), 0.5f);
                        }

                        else if(hit.transform.TryGetComponent(out Trashcan trashcan))
                        {
                            trashcan.DiscardItem();
                            PlayerHoldItem.RemoveItem();
                            _animator.SetBool("isThrow", true);
                            Invoke(nameof(ResetThrowAnimation), 0.5f);
                        }
                    }

                    if (_currentCookingMachine != null)
                    {
                        _playerInput.SwitchCurrentActionMap(_actionMapQTE);
                        _currentCookingMachine.ConvertItem(ItemInHand, this);
                    }
                    
                    else if (_currentCauldron != null && _currentCauldron.GetComponent<CauldronTimer>().CanAction)
                    {
                        if(ItemInHand.Usage == ItemUsage.Whole &&  _currentCauldron.NeedItem)
                        {
                            _currentCauldron.AddIngredient(ItemInHand);
                            PlayerHoldItem.RemoveItem();
                        }
                    }
                }
                else
                {
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        if(hit.transform.TryGetComponent(out InventoryStoragePotion inventory))
                        {
                            inventory.OpenInventory();
                            _playerInput.SwitchCurrentActionMap(_actionMapUI);
                            GetComponent<PlayerMovement>().OpenInventory();
                        }

                        else if(hit.transform.TryGetComponent(out WaitingTable waitingTable))
                        {
                            _playerInput.SwitchCurrentActionMap(_actionMapWaitingTable);
                            waitingTable.StartHighlight();
                        }
                        else if(hit.transform.TryGetComponent(out CauldronRecipeChecker cauldron) && cauldron.GetComponent<CauldronTimer>().CanAction)
                        {
                            if(!cauldron.NeedMix) return;
                            _playerInput.SwitchCurrentActionMap(_actionMapQTE);
                            cauldron.Mix(this);
                        }
                    }
                }
                
            }
        }

        private void ResetPutAnimation()
        {
            _animator.SetBool("isPut", false);
        }

        private void ResetThrowAnimation()
        {
            _animator.SetBool("isThrow", false);
        }

        public void QuitInteraction()
        {
            _playerInput.SwitchCurrentActionMap(_actionMapPlayer);
            GetComponent<PlayerMovement>().CloseInventory();
        }
    }
}