using System;
using UnityEngine;

namespace MoonlitMixes.Events
{
    [CreateAssetMenu(fileName = "ScriptableTriggerEvent", menuName = "Scriptable Objects/ScriptableTriggerEvent")]
    public class ScriptableTriggerEvent : ScriptableObject
    {
        public event Action OnTrigger;

        public void SendTrigger()
        {
            OnTrigger?.Invoke();
        }
    }
}
