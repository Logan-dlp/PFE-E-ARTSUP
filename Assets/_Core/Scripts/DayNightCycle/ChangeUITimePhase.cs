using MoonlitMixes.Datas;
using MoonlitMixes.Events;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.DayNightCycle
{
    public class ChangeUITimePhase : MonoBehaviour
    {
        [SerializeField] private Sprite[] _spriteTimePhase;
        [SerializeField] private Sprite[] _spriteDayPhase;
        [SerializeField] private Image _imageTimePhase;
        [SerializeField] private Image _imageDayPhase;
        [SerializeField] private ScriptableintEvent _scriptableIntEventTime;
        [SerializeField] private ScriptableintEvent _scriptableIntEventDay;
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;

        private void OnEnable()
        {
            _scriptableIntEventTime.OnEvent += ChangeTimePhase;
            _scriptableIntEventDay.OnEvent += ChangeDayPhase;
            ChangeTimePhase(_dayNightCycleInfo.ActualTimePhase);
            ChangeDayPhase(_dayNightCycleInfo.ActualDay);
        }

        private void OnDisable()
        {
            _scriptableIntEventTime.OnEvent -= ChangeTimePhase;
            _scriptableIntEventDay.OnEvent -= ChangeDayPhase;
        }

        private void ChangeTimePhase(int phase)
        {
            _imageTimePhase.sprite = _spriteTimePhase[phase];
        }

        private void ChangeDayPhase(int phase)
        {
            _imageDayPhase.sprite = _spriteDayPhase[phase];
        }

        [ContextMenu("IncreaseTimePhase")]
        private void IncreaseTimePhase()
        {
            if (_dayNightCycleInfo.ActualTimePhase == 3)
            {
                IncreaseDay();
            }
            else
            {
                _dayNightCycleInfo.ActualTimePhase++;
                ChangeTimePhase(_dayNightCycleInfo.ActualTimePhase);
            }
        }

        [ContextMenu("IncreaseDay")]
        private void IncreaseDay()
        {
            _dayNightCycleInfo.ActualTimePhase = 0;
            _dayNightCycleInfo.ActualDay++;
            ChangeTimePhase(_dayNightCycleInfo.ActualTimePhase);
            ChangeDayPhase(_dayNightCycleInfo.ActualDay);
        }
    }
}
