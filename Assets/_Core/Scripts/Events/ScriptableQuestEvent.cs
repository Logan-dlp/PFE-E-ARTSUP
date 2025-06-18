using System;
using MoonlitMixes.Quest;
using UnityEngine;

namespace MoonlitMixes.Events
{
    [CreateAssetMenu(fileName = "ScriptableQuestEvent", menuName = "Scriptable Objects/Event/ScriptableQuestEvent")]
    public class ScriptableQuestEvent : ScriptableObject
    {
        public event Action<QuestInfo> QuestInfoEvent;

        public void SendQuestInfo(QuestInfo questInfo)
        {
            QuestInfoEvent?.Invoke(questInfo);
        }
    }
}
