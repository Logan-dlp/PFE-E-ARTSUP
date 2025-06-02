using MoonlitMixes.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.Scene
{
    public class EndDay : OpenCanvasSceneChange
    {
        [SerializeField] private GameObject _endDayCanvasWithSell;
        [SerializeField] private GameObject _endDayCanvasWithoutSell;

        public override void OpenCanvas()
        {
            if (_dayNightCycleInfo.ActualTimePhase == 2)
            {
                _endDayCanvasWithoutSell.SetActive(true);
                InputManager.Instance.SwitchActionMap("ChangeDay");
            }
            else if (_dayNightCycleInfo.ActualTimePhase == 3)
            {
                _endDayCanvasWithSell.SetActive(true);
                InputManager.Instance.SwitchActionMap("ChangeDay");
            }
        }

        public void ChangeDay(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                _endDayCanvasWithSell.SetActive(false);
                _endDayCanvasWithoutSell.SetActive(false);
                _dayNightCycleInfo.ActualDay++;
                _dayNightCycleInfo.ActualTimePhase = 0;
                _scriptableIntEventTimePhase.SendEvent(_dayNightCycleInfo.ActualTimePhase);
                _scriptableIntEventDay.SendEvent(_dayNightCycleInfo.ActualDay);

                if (!_endDayCanvasWithSell.activeInHierarchy && !_endDayCanvasWithoutSell.activeInHierarchy)
                {
                    InputManager.Instance.SwitchActionMap("Player");
                }
            }
        }

        public void CloseCanvas()
        {
            _endDayCanvasWithSell.SetActive(false);
            _endDayCanvasWithoutSell.SetActive(false);
            InputManager.Instance.SwitchActionMap("Player");
        }
    }
}
