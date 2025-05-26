using UnityEngine;

namespace MoonlitMixes.Interactions.Objects
{
    using Inventory;
    
    public class ExplorationChest : InteractionObject
    {
        private ChestInteraction _chestInteraction;

        protected override void Awake()
        {
            _chestInteraction = GetComponent<ChestInteraction>();
            base.Awake();
        }

        public override GameObject Interact()
        {
            _chestInteraction.OpenChest();
            return gameObject;
        }
    }
}