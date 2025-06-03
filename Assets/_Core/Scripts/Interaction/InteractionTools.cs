using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Interactions
{
    using Item;
    using ExplorationTools;
    
    public class InteractionTools : Interaction
    {
        private UseTools _useTools;
        private RouletteSelectionTools _rouletteSelectionTools;
        
        private void Awake()
        {
            _useTools = FindFirstObjectByType<UseTools>();
            _rouletteSelectionTools = FindFirstObjectByType<RouletteSelectionTools>();
        }

        protected override void CheckInteraction()
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
                            
                            if (interact.GetToolType() == _rouletteSelectionTools.CurrentToolType || interact.GetToolType() == ToolType.Hand)
                            {
                                CurrentInteraction = interact;
                                CurrentInteraction.EnableUI();
                            }
                            else
                            {
                                CurrentInteraction = null;
                            }
                        }
                    }
                    else
                    {
                        if (interact.GetToolType() == _rouletteSelectionTools.CurrentToolType || interact.GetToolType() == ToolType.Hand)
                        {
                            CurrentInteraction = interact;
                            CurrentInteraction.EnableUI();
                        }
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
                if (CurrentInteraction.GetToolType() != ToolType.Hand && CurrentInteraction.GetToolType() != _rouletteSelectionTools.CurrentToolType)
                {
                    CurrentInteraction.DisableUI();
                    CurrentInteraction = null;
                }
            }
        }

        /// <summary>
        /// As Hand Interact.
        /// </summary>
        /// <param name="ctx">input</param>
        public override void InteractWithHand(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                if (CurrentInteraction != null)
                {
                    if (CurrentInteraction.GetToolType() == ToolType.Hand)
                    {
                        _useTools.UseHand(CurrentInteraction.Interact().GetComponent<ItemListSource>());
                        CurrentInteraction = null;
                    }
                }
            }
        }
    }
}