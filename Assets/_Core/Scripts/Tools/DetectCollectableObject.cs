using MoonlitMixes.ExplorationTools;
using MoonlitMixes.Interactions;
using UnityEngine;

public class DetectCollectableObject : MonoBehaviour
{
    [SerializeField] GameObject _tipsObject;
    [SerializeField] GameObject _macheteImage;
    [SerializeField] GameObject _pickaxeImage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InteractionObject>(out InteractionObject interactionObject))
        {
            if (interactionObject.GetToolType()==ToolType.Machete) 
            {
                _tipsObject.SetActive(true);
                _macheteImage.SetActive(true);
            }
            if (interactionObject.GetToolType() == ToolType.Pickaxe)
            {
                _tipsObject.SetActive(true);
                _pickaxeImage.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<InteractionObject>(out InteractionObject interactionObject))
        {
            if (interactionObject.GetToolType() == ToolType.Machete)
            {
                _tipsObject.SetActive(false);
                _macheteImage.SetActive(false);
            }
            if (interactionObject.GetToolType() == ToolType.Pickaxe)
            {
                _tipsObject.SetActive(false);
                _pickaxeImage.SetActive(false);
            }
        }
    }
}
