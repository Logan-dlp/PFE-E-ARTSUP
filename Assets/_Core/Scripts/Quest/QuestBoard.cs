using MoonlitMixes.Datas;
using MoonlitMixes.DayNightCycle;
using MoonlitMixes.Events;
using UnityEngine;

namespace MoonlitMixes.Quest
{
    public class QuestBoard : MonoBehaviour
    {
        public static bool _hasQuestBeenSendToday;

        [SerializeField] private DayNightCycleInfo dayNightCycleInfo;
        [SerializeField] private QuestInfo[] questInfosArray;
        [SerializeField] private ScriptableQuestHolder scriptableQuestEvent;
        [SerializeField] private GameObject _3DModelOutline;
        [SerializeField] private GameObject _UIIntegration;
        [SerializeField] private Collider _UItrigger;
        [SerializeField] private ScriptableBoolEvent scriptableBoolEventMenu;

        public void LoadQuestBoard()
        {
            scriptableQuestEvent.QuestInfo = null;          
            _3DModelOutline.SetActive(true);
            _UItrigger.enabled = true;
        } 

        public void TakeQuest()
        {
            if (dayNightCycleInfo.ActualTimePhase == (int)EnumDayPhase.Day && !_hasQuestBeenSendToday)
            {
                _UIIntegration.SetActive(false);
                scriptableQuestEvent.QuestInfo = questInfosArray[dayNightCycleInfo.ActualDay];
                _hasQuestBeenSendToday = true;
                _3DModelOutline.SetActive(false);
                scriptableBoolEventMenu.SendBool(true);
                _UItrigger.enabled = false;
            }
        }
    }
}
