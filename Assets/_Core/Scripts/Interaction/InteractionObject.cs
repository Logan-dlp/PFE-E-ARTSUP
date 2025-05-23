using UnityEngine;

namespace MoonlitMixes.Interactions
{
    using ExplorationTools;
    
    public abstract class InteractionObject : MonoBehaviour, IInteraction
    {
        [SerializeField] private ToolType _toolType;
        
        public ToolType GetToolType()
        {
            return _toolType;
        }

        public abstract void EnableUI();
        public abstract void DisableUI();
        public abstract void Interact();
    }
}