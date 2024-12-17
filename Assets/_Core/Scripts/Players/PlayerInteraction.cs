using MoonlitMixes.CookingMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private GameObject _itemInHand;
        [SerializeField] private float _interactionDistance;
        [SerializeField] private LayerMask _layerHitable;
        [SerializeField] private string _actionMapPlayer;
        [SerializeField] private string _actionMapWaitingTable;

        public GameObject ItemInHand
        {
            get => _itemInHand;
            set => _itemInHand = value;
        }
        private PlayerHoldItem _playerHoldItem;
        private ACookingMachine _currentCookingMachine;
        private PlayerInput _playerInput;

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
                if(_itemInHand != null)
                {
                    if (hit.transform.TryGetComponent<ACookingMachine>(out ACookingMachine cookingMachine) && cookingMachine.TransformType == _itemInHand.GetComponent<ItemDataHolder>().ItemData.Usage)
                    {
                        if (_currentCookingMachine != cookingMachine)
                        {
                            SetNewCookingMachine(cookingMachine);
                        }
                    }
                    else if (_currentCookingMachine != null)
                    {
                        _currentCookingMachine.TogleShowInteractivity();
                        _currentCookingMachine = null;
                    }
                }
                else if (_currentCookingMachine != null)
                {
                    _currentCookingMachine.TogleShowInteractivity();
                    _currentCookingMachine = null;
                }
            }
        }

        private void SetNewCookingMachine(ACookingMachine newCookingMachine)
        {
            if (_currentCookingMachine != null)
            {
                _currentCookingMachine.TogleShowInteractivity();
            }
            newCookingMachine.TogleShowInteractivity();
            _currentCookingMachine = newCookingMachine;
        }

        public void Interact(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                if(_itemInHand != null)
                {
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        if(hit.transform.TryGetComponent<WaitingTable>(out WaitingTable waitingTable) && waitingTable.CheckAvailablePlace())
                        {
                            waitingTable.PlaceItem(_playerHoldItem.ItemHold);
                            _playerHoldItem.Item = null;
                            _playerHoldItem.ItemHold = null;
                            _itemInHand = null;
                        }
                    }
                    if (_currentCookingMachine != null)
                    {
                        _itemInHand = _currentCookingMachine.ConvertItem(_itemInHand.GetComponent<ItemDataHolder>().ItemData).ItemPrefab;
                    }
                }
                else
                {
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance, _layerHitable))
                    {
                        if(hit.transform.TryGetComponent(out ProtoItemGiver itemGiver))
                        {
                            _itemInHand = itemGiver.GiveItem();
                            _playerHoldItem.GetItemData(_itemInHand);
                        }
                    }
                }
                
            }
        }

        public void InteractSecondary(InputAction.CallbackContext ctx)
        {
            if(!ctx.started) return;

            if(_itemInHand != null) return;

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