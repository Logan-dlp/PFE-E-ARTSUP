using MoonlitMixes.Datas;
using UnityEngine;

namespace MoonlitMixes.DayNightCycle
{
    public class DayCycleLoader : MonoBehaviour
    {
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;
        [SerializeField] private DayCycleLights _dayCycleLights;

        void Start()
        {
            switch(_dayNightCycleInfo.ActualTimePhase)
            {
                case 1 :
                    RenderSettings.skybox = _dayNightCycleInfo.skyboxDay;
                    _dayCycleLights.TurnOnDayLight();
                    break;
                case 2 :
                    RenderSettings.skybox = _dayNightCycleInfo.skyBoxAfternoon;
                    _dayCycleLights.TurnOnAfternoonLight();
                    break;
                case 3 :
                    RenderSettings.skybox = _dayNightCycleInfo.skyBoxTwillight;
                    _dayCycleLights.TurnOnTwillightLight();
                    break;
                case 4 :
                    RenderSettings.skybox = _dayNightCycleInfo.skyboxNight;
                    _dayCycleLights.TurnOnNightLight();
                    break;
                default :
                    RenderSettings.skybox = _dayNightCycleInfo.skyboxDawn;
                    _dayNightCycleInfo.ActualTimePhase = 0;
                    _dayCycleLights.TurnOnDawnLight();
                    break;
            }
            DynamicGI.UpdateEnvironment();
        }
    }
}
