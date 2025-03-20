using System;
using UnityEngine;

namespace MoonlitMixes.Events
{
    [CreateAssetMenu(fileName = "ScriptableEvent", menuName = "Scriptable Objects/ScriptableEvent")]
    public class ScriptableEvent : ScriptableObject
    {
        public event Action OnEvent;

        [ContextMenu("SendItems")]
        public void SendEvent()
        {
            OnEvent?.Invoke();
        }
    }
}
