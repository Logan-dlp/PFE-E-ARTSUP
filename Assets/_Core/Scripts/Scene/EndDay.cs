    using MoonlitMixes.DayNightCycle;
using MoonlitMixes.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Scene
{
    public class EndDay : OpenCanvasSceneChange
    {
        [SerializeField] private EnumDayPhase _requiredTimePhaseToSleep;

        public override void OpenCanvas()
        {
            if (_dayNightCycleInfo.ActualTimePhase == (int)_requiredTimePhaseToSleep)
            {
                InputManager.Instance.SwitchActionMap("ChangeDay");
            }
            else if (_dayNightCycleInfo.ActualTimePhase == 1 + (int)_requiredTimePhaseToSleep)
            {
                InputManager.Instance.SwitchActionMap("ChangeDay");
            }
        }

        public void ChangeDay(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                _dayNightCycleInfo.ActualDay++;
                _dayNightCycleInfo.ActualTimePhase = 0;
                _scriptableIntEventTimePhase.SendEvent(_dayNightCycleInfo.ActualTimePhase);
                _scriptableIntEventDay.SendEvent(_dayNightCycleInfo.ActualDay);
                
                InputManager.Instance.SwitchActionMap("Player");
            }
        }
    }
}
