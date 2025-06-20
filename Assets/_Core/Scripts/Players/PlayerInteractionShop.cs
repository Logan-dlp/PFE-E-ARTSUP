using MoonlitMixes.AI.PNJ;
using MoonlitMixes.Datas;
using MoonlitMixes.Scene;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Player
{
    public class PlayerInteractionShop : MonoBehaviour
    {
        [SerializeField] private float _interactionDistance;
        [SerializeField] private LayerMask _layerHitable;
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;

        public void Interact(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                if (Physics.Raycast(transform.position, transform.forward + new Vector3(0, 1, 0), out RaycastHit hit, _interactionDistance, _layerHitable))
                {
                    Debug.Log(hit.transform.tag);
                    if (hit.transform.tag == "Register")
                    {
                        hit.transform.GetComponent<CloseOrOpenShop>().OnToggleShop();
                    }
                    else if (hit.transform.TryGetComponent(out DoorSceneChange doorSceneChange))
                    {
                        Debug.Log($"Touched door with scene: {doorSceneChange.SceneName}");
                        Debug.Log($"Phase actuelle : {_dayNightCycleInfo.ActualTimePhase}");

                        if (doorSceneChange.SceneName == "S_Labo")
                        {
                            doorSceneChange.OpenCanvas();
                        }
                        
                        else if (doorSceneChange.SceneName != "S_Labo")
                        {
                            doorSceneChange.OpenCanvas();
                        }
                    }
                }
            }
        }
    }
}
