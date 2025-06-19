using UnityEngine;

namespace MoonlitMixes.Interactions.Objects
{
    using Scene;
    
    public class DoorHub : InteractionObject
    {
        private DoorSceneChange _doorInteraction;

        protected override void Awake()
        {
            _doorInteraction = GetComponent<DoorSceneChange>();
            base.Awake();
        }

        public override GameObject Interact()
        {
            _doorInteraction.OpenCanvas();
            return gameObject;
        }
    }
}

