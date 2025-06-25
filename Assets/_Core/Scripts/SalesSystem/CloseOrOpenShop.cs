using MoonlitMixes.AI.PNJ.Spawner;
using MoonlitMixes.Datas;
using MoonlitMixes.DayNightCycle;
using MoonlitMixes.Dialogue;
using MoonlitMixes.Events;
using MoonlitMixes.Tutorial;
using System;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ
{
    public class CloseOrOpenShop : MonoBehaviour
    {
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;
        [SerializeField] private EnumDayPhase _timePhaseRequired;
        [SerializeField] private ScriptableintEvent _scriptableintEvent;
        [SerializeField] private TutorialSaveInfo _tutorialSaveInfo;

        public static event Action OnShopUIShouldDeactivate;
        public static event Action<bool> OnShopToggled;

        public void OnToggleShop()
        {
            if (_dayNightCycleInfo.ActualTimePhase == (int)_timePhaseRequired)
            {
                if (_tutorialSaveInfo != null)
                {
                    if (!_tutorialSaveInfo.tutorialShopNightDone)
                    {
                        Debug.Log("Tuto");
                        FindFirstObjectByType<TutorialActivation>().TutorialShopTwilightPart2();
                        return;
                    }
                }
            
                OnShopToggled?.Invoke(true);
                CustomerSpawner.RequestSpawning();
                OnShopUIShouldDeactivate?.Invoke();
                _scriptableintEvent.SendEvent(_dayNightCycleInfo.ActualTimePhase);
            }
        }
    }
}