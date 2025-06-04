using MoonlitMixes.DayNightCycle;
using MoonlitMixes.Inputs;
using MoonlitMixes.UI;
using UnityEngine;

namespace MoonlitMixes.Scene
{
    public class DoorSceneChange : OpenCanvasSceneChange
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private EnumDayPhase _timePhaseRequired;
        [SerializeField] private bool _increaseTimePhase;
        [SerializeField] private bool _needTimePhase;

        public string SceneName
        {
            get => _sceneName;
        }

        public override void OpenCanvas()
        {
            var canvasUI = FindFirstObjectByType<ChangeSceneUI>();
            if (canvasUI != null)
            {
                if (_needTimePhase && _dayNightCycleInfo.ActualTimePhase != (int)_timePhaseRequired)
                {
                    return;
                }
                canvasUI.OpenCanvas(_sceneName, _increaseTimePhase);
                InputManager.Instance.SwitchActionMap("ChangeScene");
            }
            else if (canvasUI == null)
            {
                Debug.LogError("ChangeSceneUI est introuvable dans la scène !");
            }
        }
    }
}
