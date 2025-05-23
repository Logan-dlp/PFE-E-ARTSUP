using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Interactions
{
    public class Interaction : MonoBehaviour
    {
        [SerializeField] protected float _interactDistance;
        [SerializeField] protected LayerMask _interactionMask;
        [SerializeField] protected Vector3 _raycastOffset;
        
        protected IInteraction CurrentInteraction;

        private void Update()
        {
            CheckInteraction();
        }

        protected virtual void CheckInteraction()
        {
            if (Physics.Raycast(transform.position + _raycastOffset, transform.forward, out RaycastHit hit, _interactDistance, _interactionMask))
            {
                if (hit.transform.TryGetComponent(out IInteraction interact))
                {
                    if (CurrentInteraction != null)
                    {
                        if (CurrentInteraction != interact)
                        {
                            CurrentInteraction.DisableUI();
                            
                            CurrentInteraction = interact;
                            CurrentInteraction.EnableUI();
                        }
                    }
                    else
                    {
                        CurrentInteraction = interact;
                        CurrentInteraction.EnableUI();
                    }
                    
                }
            }
            else
            {
                if (CurrentInteraction != null)
                {
                    CurrentInteraction.DisableUI();
                    CurrentInteraction = null;
                }
            }

            if (CurrentInteraction != null)
            {
                CurrentInteraction.DisableUI();
                CurrentInteraction = null;
            }
        }

        public virtual void InteractWithHand(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                if (CurrentInteraction != null)
                {
                    CurrentInteraction.Interact();
                }
            }
        }
    }
}