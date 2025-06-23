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
            _dayNightCycleInfo = Resources.Load<DayNightCycleInfo>("DayNightCycleInfo");
            _dayNightCycleInfo.ActualTimePhase = (int)EnumDayPhase.Day;
            _dayNightCycleInfo.ActualDay = 0;
        }
    }
}
