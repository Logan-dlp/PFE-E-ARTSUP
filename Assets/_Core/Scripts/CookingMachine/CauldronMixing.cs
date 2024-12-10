using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MoonlitMixes.CookingMachine
{
    public class CauldronMixing : ACookingMachine
    {
        [SerializeField] private GameObject InteractUI;
        [SerializeField] private GameObject ProgressUI;
        [SerializeField] private float InteractionDuration = 2f;

        private Slider ProgressSlider;
        private bool _isInteracting = false;

        private void Awake()
        {
            ProgressSlider = GetComponentInChildren<Slider>(true);
        }

        public override void TogleShowInteractivity()
        {
            if (_isInteracting) return;

            InteractUI.SetActive(true);
        }

        public void StartInteraction()
        {
            if (_isInteracting) return;

            _isInteracting = true;
            ProgressUI.SetActive(true);
            ProgressSlider.value = 0f;

            StartCoroutine(HandleInteraction());
        }

        private IEnumerator HandleInteraction()
        {
            ProgressUI.SetActive(true);
            InteractUI.SetActive(false);
            ProgressSlider.value = 0f;

            float elapsedTime = 0f;

            while (elapsedTime < InteractionDuration)
            {
                elapsedTime += Time.deltaTime;
                ProgressSlider.value = Mathf.Clamp01(elapsedTime / InteractionDuration);
                yield return null;
            }

            ProgressSlider.value = 1f;
            ProgressUI.SetActive(false);
            _isInteracting = false;
        }
    }
}