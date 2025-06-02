using MoonlitMixes.Datas;
using UnityEngine;

namespace MoonlitMixes.DayNightCycle
{
    public class DayCycleBakedLoader : MonoBehaviour
    {
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;
        [SerializeField] private DayCycleLightBaked _dayCycleLights;

        void Start()
        {
            switch(_dayNightCycleInfo.ActualTimePhase)
            {
                case 1 :
                    RenderSettings.skybox = _dayNightCycleInfo.SkyboxDay;
                    break;
                case 2 :
                    RenderSettings.skybox = _dayNightCycleInfo.SkyBoxAfternoon;
                    break;
                case 3 :
                    RenderSettings.skybox = _dayNightCycleInfo.SkyBoxTwillight;
                    break;
                case 4 :
                    RenderSettings.skybox = _dayNightCycleInfo.SkyboxNight;
                    break;
                default :
                    RenderSettings.skybox = _dayNightCycleInfo.SkyboxDawn;
                    _dayNightCycleInfo.ActualTimePhase = 0;
                    break;
            }

            //_dayCycleLights.ChangeBake(_dayNightCycleInfo.ActualTimePhase);
        }
    }
}
