using System;
using MoonlitMixes.Potion;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptablePotionResultEvent", menuName = "Scriptable Objects/ScriptablePotionResultEvent")]
public class ScriptablePotionResultEvent : ScriptableObject
{
    public Action<PotionResult> OnPotionResultEvent;

    public void SendPotionResult(PotionResult potionResult)
    {
        OnPotionResultEvent?.Invoke(potionResult);
    }
}
