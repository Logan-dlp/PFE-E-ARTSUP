using System;
using MoonlitMixes.Item;
using UnityEngine;

namespace MoonlitMixes.Events
{
    [CreateAssetMenu(fileName = "ScriptableItemUsageEvent", menuName = "Scriptable Objects/ScriptableItemUsageEvent")]
    public class ScriptableItemUsageEvent : ScriptableObject
    {
        public event Action<ItemUsage> OnItemUsageEvent;

        public void SendEvent(ItemUsage itemUsage)
        {
            OnItemUsageEvent?.Invoke(itemUsage);
        }
    }
}
