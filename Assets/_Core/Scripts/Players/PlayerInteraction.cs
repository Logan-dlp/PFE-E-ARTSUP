using MoonlitMixes.CookingMachine;
using MoonlitMixes.Item;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance;
        [SerializeField] private LayerMask _layerHitable;
        [SerializeField] private string _actionMapPlayer;
        [SerializeField] private string _actionMapWaitingTable;

        public GameObject ItemInHand { get; set; }
        
        private PlayerHoldItem _playerHoldItem;
        private ACookingMachine _currentCookingMachine;
        private PlayerInput _playerInput;
        private CauldronRecipeChecker _currentCauldron;

        private void Awake()
        {
            _playerHoldItem = GetComponent<PlayerHoldItem>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            Debug.DrawRay(transform.position, transform.forward * _interactionDistance, Color.red);
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance))
            {
                if(ItemInHand != null)
                {
                    if (hit.transform.TryGetComponent<ACookingMachine>(out ACookingMachine cookingMachine) && cookingMachine.TransformType == ItemInHand.GetComponent<ItemDataHolder>().ItemData.Usage)
                    {
                        if (_currentCookingMachine != cookingMachine)
                        {
                            SetNewCookingMachine(cookingMachine);
                        }
                    }

                    else if(hit.transform.TryGetComponent<CauldronRecipeChecker>(out CauldronRecipeChecker cauldron))
                    {
                        if (_currentCauldron != cauldron)
                        {
                            SetNewCauldron(cauldron);
                        }
                    }

                    else if (_currentCookingMachine != null)
                    {
                        _currentCookingMachine.TogleShowInteractivity();
                        _currentCookingMachine = null;
                        ResetInteractionTargets();
                    }
                }
                else
                {
                    ResetInteractionTargets();
                }
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
            if (_currentCauldron != null)
            {
                _currentCauldron.TogleShowInteractivity();
                _currentCauldron = null;
            }

            if (_currentCookingMachine != null)
            {
                _currentCookingMachine.TogleShowInteractivity();
                _currentCookingMachine = null;
            }
        }

        public void Interact(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {

                if(ItemInHand != null)
                {
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        if(hit.transform.TryGetComponent<WaitingTable>(out WaitingTable waitingTable) && waitingTable.CheckAvailablePlace())
                        {
                            waitingTable.PlaceItem(_playerHoldItem.ItemHold);
                            _playerHoldItem.Item = null;
                            _playerHoldItem.ItemHold = null;
                            ItemInHand = null;
                        }
                    }

                    if (_currentCookingMachine != null)
                    {
                        ItemInHand = _currentCookingMachine.ConvertItem(ItemInHand.GetComponent<ItemDataHolder>().ItemData).ItemPrefab;
                    
                    }
                    
                    else if (_currentCauldron != null)
                    {
                        _currentCauldron.AddIngredient(ItemInHand.GetComponent<ItemDataHolder>().ItemData);
                        ItemInHand = null;
                    }
    
                    else
                    {
                        Debug.Log("Il faut attendre avant d'ajouter un autre ingrédient !");
                    }
                }
                else
                {
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        if(hit.transform.TryGetComponent(out ProtoItemGiver itemGiver))
                        {
                            ItemInHand = itemGiver.GiveItem();
                            _playerHoldItem.GetItemData(ItemInHand);
                        }
                    }
                    else
                    {
                        Debug.Log("Vous n'avez aucun objet dans les mains !");
                    }
                }
                
            }
        }

        public void InteractSecondary(InputAction.CallbackContext ctx)
        {
            if(!ctx.started) return;

            if(ItemInHand != null) return;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _layerHitable))
            {
                if(hit.transform.TryGetComponent(out WaitingTable waitingTable) && waitingTable.CheckAvailablePlace())
                {
                    _playerInput.actions.FindActionMap(_actionMapPlayer).Disable();
                    _playerInput.actions.FindActionMap(_actionMapWaitingTable).Enable();
                    waitingTable.StartHighlight();
                }
            }
        }
    }
}