using MoonlitMixes.AI.PNJ.Spawner;
using MoonlitMixes.Datas;
using MoonlitMixes.DayNightCycle;
using MoonlitMixes.Events;
using System;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ
{
    public class CloseOrOpenShop : MonoBehaviour
    {
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;
        [SerializeField] private EnumDayPhase _timePhaseRequired;
        [SerializeField] private ScriptableintEvent _scriptableintEvent;

        public static event Action OnShopUIShouldDeactivate;
        public static event Action<bool> OnShopToggled;

        public void OnToggleShop()
        {
            if (_dayNightCycleInfo.ActualTimePhase == (int)_timePhaseRequired)
            {
                OnShopToggled?.Invoke(true);
                CustomerSpawner.RequestSpawning();
                OnShopUIShouldDeactivate?.Invoke();
                _scriptableintEvent.SendEvent(_dayNightCycleInfo.ActualTimePhase);
            }
        }
    }
}