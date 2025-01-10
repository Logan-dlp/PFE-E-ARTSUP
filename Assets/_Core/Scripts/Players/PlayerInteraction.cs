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
        private CauldronRecipeChecker _currentCauldron;

        private void Update()
        {
            Debug.DrawRay(transform.position, transform.forward * _interactionDistance, Color.red);

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _interactionDistance))
            {
                if (_itemDataInHand != null)
                {
                    if (hit.transform.TryGetComponent<CauldronRecipeChecker>(out CauldronRecipeChecker cauldron))
                    {
                        if (_currentCauldron != cauldron)
                        {
                            SetNewCauldron(cauldron);
                        }
                    }
                    else if (hit.transform.TryGetComponent<ACookingMachine>(out ACookingMachine cookingMachine))
                    {
                        if (_currentCookingMachine != cookingMachine)
                        {
                            SetNewCookingMachine(cookingMachine);
                        }
                    }
                    else
                    {
                        ResetInteractionTargets();
                    }
                }
                else
                {
                    ResetInteractionTargets();
                }
            }
            else
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
                if (_itemDataInHand != null)
                {
                        if (_currentCauldron != null)
                        {
                            _currentCauldron.AddIngredient(_itemDataInHand);
                            _itemDataInHand = null;
                        }
                        else if (_currentCookingMachine != null)
                        {
                            _itemDataInHand = _currentCookingMachine.ConvertItem(_itemDataInHand);
                        }
                    else
                    {
                        Debug.Log("Il faut attendre avant d'ajouter un autre ingrédient !");
                    }
                }
                else
                {
                    Debug.Log("Vous n'avez aucun objet dans les mains !");
                }
            }
        }
    }
}