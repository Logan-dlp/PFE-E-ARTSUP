using UnityEngine;
using MoonlitMixes.Datas;

namespace MoonlitMixes.DayNightCycle
{
    public class UpdateDayCycle : MonoBehaviour
    {
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;

        public void UpdateDayPhase()
        {
            _dayNightCycleInfo.ActualTimePhase++;
        }
    }
}
