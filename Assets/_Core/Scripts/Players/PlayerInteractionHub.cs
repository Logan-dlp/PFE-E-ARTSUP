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

        private bool _hasChest;

        public void Interact(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                if(_hasChest == true)
                {
                    _chestInteraction.OpenChest();
                }
                else if (Physics.Raycast(transform.position, transform.forward + new Vector3(0, 1, 0), out RaycastHit hit, _interactionDistance, _layerHitable))
                {
                    if (hit.transform.TryGetComponent(out DoorSceneChange doorSceneChange))
                    {
                        doorSceneChange.OpenCanvas();
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out TriggerButtonUI _))
            {
                _hasChest = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out TriggerButtonUI _))
            {
                _hasChest = false;
            }
        }
    }
}