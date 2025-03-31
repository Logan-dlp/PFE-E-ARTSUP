using System;
using UnityEngine;

namespace MoonlitMixes.Events
{
    [CreateAssetMenu(fileName = "BoolEvent", menuName = "Scriptable Objects/BoolEvent")]
    public class ScriptableBoolEvent : ScriptableObject
    {
        public event Action<bool> BoolAction;
        
        public void SendBool(bool boolValue)
        {
            BoolAction?.Invoke(boolValue);
        }
    }
}