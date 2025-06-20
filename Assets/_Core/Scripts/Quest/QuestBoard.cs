using MoonlitMixes.Datas;
using MoonlitMixes.DayNightCycle;
using MoonlitMixes.Events;
using MoonlitMixes.Player.Interaction;
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

        private void OnEnable()
        {
            scriptableQuestEvent.QuestInfo = null;
            Debug.Log(_hasQuestBeenSendToday);
            if (dayNightCycleInfo.ActualTimePhase == (int)EnumDayPhase.Day && !_hasQuestBeenSendToday)
            {
                _3DModelOutline.SetActive(true);
            }
            else
            {
                _3DModelOutline.SetActive(false);
                _UItrigger.enabled = false;
            }
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
