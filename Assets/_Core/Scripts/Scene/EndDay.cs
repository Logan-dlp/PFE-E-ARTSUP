using MoonlitMixes.DayNightCycle;
using MoonlitMixes.Inputs;
using MoonlitMixes.Quest;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Scene
{
    public class EndDay : OpenCanvasSceneChange
    {
        [SerializeField] private EnumDayPhase _requiredTimePhaseToSleep;
        [SerializeField] private string _sceneToLoad;
        [SerializeField] private Animator _animator;

        public override void OpenCanvas()
        {
            if (_dayNightCycleInfo.ActualTimePhase == (int)_requiredTimePhaseToSleep)
            {
                ChangeDay();
            }
        }

        public void ChangeDay()
        {
            QuestBoard._hasQuestBeenSendToday = false;
            _dayNightCycleInfo.ActualDay++;
            _dayNightCycleInfo.ActualTimePhase = 0;
            SceneLoader.LoadAsyncScene(_sceneToLoad, _animator);
            _scriptableIntEventTimePhase.SendEvent(_dayNightCycleInfo.ActualTimePhase);
            _scriptableIntEventDay.SendEvent(_dayNightCycleInfo.ActualDay);

            FindFirstObjectByType<QuestBoard>().LoadQuestBoard();
        }
    }
}
