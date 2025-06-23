using MoonlitMixes.AI.PNJ;
using MoonlitMixes.Datas;
using MoonlitMixes.DayNightCycle;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.Player.Interaction
{
    public class InteractionButton : MonoBehaviour
    {
        [SerializeField] private Image _buttonImage;
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;
        [SerializeField] private EnumDayPhase _timePhaseRequired;

        private void Start()
        {
            if (_buttonImage != null)
            {
                _buttonImage.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_buttonImage != null && _dayNightCycleInfo.ActualTimePhase == (int)_timePhaseRequired)
            {
                _buttonImage.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_buttonImage != null)
            {
                _buttonImage.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            CloseOrOpenShop.OnShopUIShouldDeactivate += DeactivateButtonUI;
        }

        private void OnDisable()
        {
            CloseOrOpenShop.OnShopUIShouldDeactivate -= DeactivateButtonUI;
        }

        private void DeactivateButtonUI()
        {
            if (_buttonImage != null)
            {
                _buttonImage.gameObject.SetActive(false);
            }
        }
    }
}