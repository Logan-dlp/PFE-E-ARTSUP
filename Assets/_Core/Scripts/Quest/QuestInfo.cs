using System;
using MoonlitMixes.Potion;
using UnityEngine;

namespace MoonlitMixes.Quest
{
    [CreateAssetMenu(fileName = "QuestInfo", menuName = "Scriptable Objects/QuestInfo")]
    public class QuestInfo : ScriptableObject
    {
        public Quest[] quests;

        [Serializable]
        public struct Quest
        {
            public Recipe potion;
            [TextArea] public string text;
        }
    }
}
