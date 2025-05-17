using MoonlitMixes.AI.PNJ.Spawner;
using MoonlitMixes.Datas;
using System;
using UnityEngine;

namespace MoonlitMixes.AI.PNJ
{
    public class CloseOrOpenShop : MonoBehaviour
    {
        public static event Action OnShopUIShouldDeactivate;
        public static event Action<bool> OnShopToggled;

        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;

        public void OnToggleShop()
        {
            if (_dayNightCycleInfo.ActualTimePhase == 2)
            {
                OnShopToggled?.Invoke(true);
                CustomerSpawner.RequestSpawning();
                OnShopUIShouldDeactivate?.Invoke();
                _dayNightCycleInfo.ActualTimePhase++;
            }
        }
    }
}