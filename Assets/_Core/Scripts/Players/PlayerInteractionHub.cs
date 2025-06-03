using MoonlitMixes.Inventory;
using MoonlitMixes.Scene;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Player
{
    public class PlayerInteractionHub : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance;
        [SerializeField] private LayerMask _layerHitable;
        [SerializeField] private ChestInteraction _chestInteraction;

        private UseTools _useTools;
        private bool _hasChest;

        private void Start()
        {
            TryGetComponent(out UseTools useTools);
            _useTools = useTools;
        }
        
        public void Interact(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                if (Physics.Raycast(transform.position, transform.forward + new Vector3(0, 1, 0), out RaycastHit hit, _interactionDistance, _layerHitable))
                {
                    if (hit.transform.TryGetComponent(out DoorSceneChange doorSceneChange))
                    {
                        doorSceneChange.OpenCanvas();
                    }
                }

                else if(_useTools != null)
                {
                    if (_useTools.CanUseHand())
                    {
                        _useTools.UseHand();
                        return;
                    }
                
                    if (_hasChest)
                    {
                        _chestInteraction.OpenChest();
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Chest")
            {
                _hasChest = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Chest")
            {
                _hasChest = false;
            }
        }
    }
}