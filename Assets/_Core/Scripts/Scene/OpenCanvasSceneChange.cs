using MoonlitMixes.Datas;
using MoonlitMixes.Events;
using UnityEngine;

namespace MoonlitMixes.Scene
{
    public abstract class OpenCanvasSceneChange : MonoBehaviour
    {
        [SerializeField] protected DayNightCycleInfo _dayNightCycleInfo;
        [SerializeField] protected ScriptableintEvent _scriptableIntEventTimePhase;
        [SerializeField] protected ScriptableintEvent _scriptableIntEventDay;

        public abstract void OpenCanvas();
    }
}