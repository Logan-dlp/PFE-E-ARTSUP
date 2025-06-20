using System;
using UnityEngine;

namespace MoonlitMixes.Events
{
    [CreateAssetMenu(fileName = "ScriptableIntEvent", menuName = "Scriptable Objects/Event/ScriptableIntEvent")]
    public class ScriptableintEvent : ScriptableObject
    {
        public event Action<int> OnEvent;

        public void SendEvent(int value)
        {
            OnEvent?.Invoke(value);
        }
    }
}
