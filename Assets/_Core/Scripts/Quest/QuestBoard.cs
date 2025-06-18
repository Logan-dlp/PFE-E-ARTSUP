using MoonlitMixes.Datas;
using MoonlitMixes.Events;
using UnityEngine;

namespace MoonlitMixes.Quest
{
    public class QuestBoard : MonoBehaviour
    {
        [SerializeField] private DayNightCycleInfo dayNightCycleInfo;
        [SerializeField] private QuestInfo[] questInfosArray;
        [SerializeField] private ScriptableQuestEvent scriptableQuestEvent;

        public void TakeQuest()
        {
            scriptableQuestEvent.SendQuestInfo(questInfosArray[0]);
        }
    }
}
