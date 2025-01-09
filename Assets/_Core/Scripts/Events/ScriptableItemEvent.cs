using System;
using UnityEngine;

namespace MoonlitMixes.Events
{
    [CreateAssetMenu(fileName = "ItemEvent", menuName = "Scriptable Objects/ItemEvent")]
    public class ScriptableItemEvent : ScriptableObject
    {
        public event Action<GameObject> ItemDataAction;
        public void SendObject(GameObject item)
        {
            ItemDataAction?.Invoke(item);
        }
    }
}