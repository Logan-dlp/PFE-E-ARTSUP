using MoonlitMixes.Scene;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionHub : MonoBehaviour
{
    [SerializeField] private float _interactionDistance;
    [SerializeField] private LayerMask _layerHitable;

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
        }
    }
}
