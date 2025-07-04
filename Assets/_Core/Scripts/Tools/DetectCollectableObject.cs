using MoonlitMixes.ExplorationTools;
using MoonlitMixes.Interactions;
using UnityEngine;
using System.Collections.Generic;

public class DetectCollectableObject : MonoBehaviour
{
    [SerializeField] private GameObject _tipsObject;
    [SerializeField] private GameObject _macheteImage;
    [SerializeField] private GameObject _pickaxeImage;

    private Dictionary<IDamageable, ToolType> _activeDamageables = new();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out InteractionObject interactionObject)) return;
        ToolType toolType = interactionObject.GetToolType();

        if (other.TryGetComponent(out IDamageable damageable))
        {
            if (!damageable.CanInteract()) return;

            if (!_activeDamageables.ContainsKey(damageable))
            {
                _activeDamageables.Add(damageable, toolType);
                damageable.OnBecameUnusable += () => HandleBecameUnusable(damageable);
            }
        }

        ShowToolImage(toolType, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out InteractionObject interactionObject)) return;
        ToolType toolType = interactionObject.GetToolType();

        if (other.TryGetComponent(out IDamageable damageable))
        {
            if (_activeDamageables.ContainsKey(damageable))
            {
                damageable.OnBecameUnusable -= () => HandleBecameUnusable(damageable);
                _activeDamageables.Remove(damageable);
            }
        }

        ShowToolImage(toolType, false);
    }

    private void HandleBecameUnusable(IDamageable damageable)
    {
        if (!_activeDamageables.TryGetValue(damageable, out ToolType toolType)) return;

        ShowToolImage(toolType, false);
        _activeDamageables.Remove(damageable);
    }

    private void ShowToolImage(ToolType toolType, bool show)
    {
        switch (toolType)
        {
            case ToolType.Machete:
                _tipsObject.SetActive(show);
                _macheteImage.SetActive(show);
                break;
            case ToolType.Pickaxe:
                _tipsObject.SetActive(show);
                _pickaxeImage.SetActive(show);
                break;
        }
    }
}