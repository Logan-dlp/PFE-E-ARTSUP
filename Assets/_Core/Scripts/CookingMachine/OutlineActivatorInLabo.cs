using UnityEngine;
using MoonlitMixes.Datas;
using UnityEngine.SceneManagement;
using MoonlitMixes.UI;
using MoonlitMixes.Player;
using NaughtyAttributes;

namespace MoonlitMixes.DayNightCycle
{
    public class OutlineActivatorInLabo : MonoBehaviour
    {
        [SerializeField] private GameObject[] outlinesToActivate;
        [SerializeField] private DayNightCycleInfo dayNightCycleInfo;
        [SerializeField] private GameObject _verificationChangePhase;
        [SerializeField] private GameObject _textTuto;
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private GameObject _changeScene;

        private static bool hasActivatedInLabo = false;

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "S_Labo" && !hasActivatedInLabo)
            {
                if (dayNightCycleInfo.ActualTimePhase == 1)
                {
                    ActivateOutlines();
                    hasActivatedInLabo = true;
                }
            }
        }
        [Button]
        private void ActivateOutlines()
        {
            foreach (GameObject outline in outlinesToActivate)
            {
                if (outline != null)
                    outline.SetActive(true);
            }
        }

        public void DeactivateOutlines()
        {
            foreach (GameObject outline in outlinesToActivate)
            {
                if (outline != null)
                    outline.SetActive(false);
            }
        }
        public void ActivateCauldrons()
        {
            if (_verificationChangePhase.activeInHierarchy)
            {
                _verificationChangePhase.SetActive(false);
                dayNightCycleInfo.ActualTimePhase += 1;
                _playerInteraction.CanInteract = true;
                _playerInteraction.ActivateInput();
                _textTuto.SetActive(false);
                DeactivateOutlines();
            }
                
        }
        public void CancelCauldrons()
        {
            if (_verificationChangePhase.activeInHierarchy)
            {
                _verificationChangePhase.SetActive(false);
                _playerInteraction.ActivateInput();
            }
            else if(_changeScene.activeInHierarchy)
            {
                _changeScene.SetActive(false);
                _playerInteraction.ActivateInput();
            }
        }
    }
}