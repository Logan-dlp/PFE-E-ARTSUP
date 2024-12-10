using MoonlitMixes.CookingMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private ItemData _itemDataInHand;
        [SerializeField] private float _interactionDistance;

        private ACookingMachine _currentCookingMachine;

        private void Update()
        {
            Debug.DrawRay(transform.position, transform.forward * _interactionDistance, Color.red);
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance) && _itemDataInHand != null)
            {
                if (hit.transform.TryGetComponent<ACookingMachine>(out ACookingMachine cookingMachine) && cookingMachine.TransformType == _itemDataInHand.Usage)
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
            if (ctx.started && _itemDataInHand != null && _currentCookingMachine is CauldronMixing cauldron)
            {
                cauldron.AddIngredient(_itemDataInHand);

                _itemDataInHand = _currentCookingMachine.ConvertItem(_itemDataInHand);
            }
        }
    }
}