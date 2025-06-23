using MoonlitMixes.DayNightCycle;
using MoonlitMixes.Inputs;
using MoonlitMixes.Quest;
using MoonlitMixes.Scene;
using MoonlitMixes.UI;
using UnityEngine;

public class DoorSceneChangeShop : OpenCanvasSceneChange
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
        if (QuestBoard._hasQuestBeenSendToday)
        {
            var canvasUI = FindFirstObjectByType<ChangeSceneUI>();
            if (canvasUI != null)
            {
                if (_needTimePhase && _dayNightCycleInfo.ActualTimePhase != (int)_timePhaseRequired)
                {
                    return;
                }
                if (_dayNightCycleInfo.ActualTimePhase != (int)_timePhaseRequired && _increaseTimePhase)
                {
                    canvasUI.OpenCanvas(_sceneName, true);
                }
                else
                {
                    canvasUI.OpenCanvas(_sceneName, false);
                }

                InputManager.Instance.SwitchActionMap("ChangeScene-Day");
            }
            else if (canvasUI == null)
            {
                Debug.LogError("ChangeSceneUI est introuvable dans la scène !");
            }
        }
    }
}
