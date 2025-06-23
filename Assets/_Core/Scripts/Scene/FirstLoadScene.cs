using MoonlitMixes.Datas;
using MoonlitMixes.DayNightCycle;
using UnityEngine;

namespace MoonlitMixes.Scene
{
    public class FirstLoadGame : MonoBehaviour
    {
        [SerializeField] private LastSceneNameData _lastSceneNameData;
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;

        private void Start()
        {
            if (_lastSceneNameData.sceneName == "S_TitleScreen" &&
            _dayNightCycleInfo.ActualTimePhase == (int)EnumDayPhase.Day
            && _dayNightCycleInfo.ActualDay == 0)
            {
                InitGame();
            }
        }

        private void InitGame()
        {
            //Put here anything that has to be made when the
            //game first start after the title screen
        }
    }
}