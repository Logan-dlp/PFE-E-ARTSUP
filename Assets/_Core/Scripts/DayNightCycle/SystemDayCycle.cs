using UnityEngine;
using MoonlitMixes.Datas;

namespace MoonlitMixes.DayNightCycle
{
    public class SystemDayCycle : MonoBehaviour
    {
        private static DayNightCycleInfo _dayNightCycleInfo;


        [RuntimeInitializeOnLoadMethod]
        private static void GameStartup()
        {
            Debug.Log("Test");
            _dayNightCycleInfo = Resources.Load<DayNightCycleInfo>("DayNightCycleInfo");
            _dayNightCycleInfo.ActualTimePhase = 0;
            _dayNightCycleInfo.ActualDay = 0;
        }
    }
}
