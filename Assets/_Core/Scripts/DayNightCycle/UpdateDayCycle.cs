using UnityEngine;
using MoonlitMixes.Datas;

namespace MoonlitMixes.DayNightCycle
{
    public class UpdateDayCycle : MonoBehaviour
    {
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;

        public void Start()
        {
            _dayNightCycleInfo.ActualTimePhase++;
        }
    }
}
