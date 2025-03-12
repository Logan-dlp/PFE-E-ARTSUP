using MoonlitMixes.Datas;
using UnityEngine;

namespace MoonlitMixes.DayNightCycle
{
    public class DayCycleLoader : MonoBehaviour
    {
        [SerializeField] private DayNightCycleInfo dayNightCycleInfo;

        void Start()
        {
            switch(dayNightCycleInfo.ActualTimePhase)
            {
                case 0 : 
                    RenderSettings.skybox = dayNightCycleInfo.skyboxDawn;
                    break;
                case 1 :
                    RenderSettings.skybox = dayNightCycleInfo.skyboxDay;
                    break;
                case 2 :
                    RenderSettings.skybox = dayNightCycleInfo.skyBoxAfternoon;
                    break;
                case 3 :
                    RenderSettings.skybox = dayNightCycleInfo.skyBoxTwillight;
                    break;
                case 4 :
                    RenderSettings.skybox = dayNightCycleInfo.skyboxNight;
                    break;
                default :
                    RenderSettings.skybox = dayNightCycleInfo.skyboxDawn;
                    dayNightCycleInfo.ActualTimePhase = 0;
                    break;
            }
            DynamicGI.UpdateEnvironment();
        }
    }
}
