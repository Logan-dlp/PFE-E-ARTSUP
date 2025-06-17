using MoonlitMixes.Events;
using UnityEngine;

namespace MoonlitMixes.UI
{
    public class OptionPageUI : MonoBehaviour
    {
        [SerializeField] private GameObject _optionGameObject;
        [SerializeField] private GameObject _controlsGameObject;
        [SerializeField] private GameObject _creditsGameObject;
        [SerializeField] private GameObject _volumeGameObject;
        [SerializeField] private ScriptableEvent scriptableEvent;

        void OnEnable()
        {
            scriptableEvent.OnEvent += CloseOtherOptions;
        }

        void OnDisable()
        {
            scriptableEvent.OnEvent -= CloseOtherOptions;
        }

        public void OpenVolume()
        {
            _optionGameObject.SetActive(false);
            _volumeGameObject.SetActive(true); 
        }

        public void OpenControls()
        {
            _optionGameObject.SetActive(false);
            _controlsGameObject.SetActive(true);
        }
        public void OpenCredits()
        {
            _optionGameObject.SetActive(false);
            _creditsGameObject.SetActive(true);
        }
        private void CloseOtherOptions()
        {
            _volumeGameObject.SetActive(false);
            _creditsGameObject.SetActive(false);
            _controlsGameObject.SetActive(false);
            _optionGameObject.SetActive(true);
        }
    }
}