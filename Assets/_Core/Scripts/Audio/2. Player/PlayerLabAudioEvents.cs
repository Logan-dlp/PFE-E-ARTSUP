using FMODUnity;
using FMOD.Studio;
using UnityEngine;

namespace MoonlitMixes.Audio
{
    public class PlayerLabAudioEvents : MonoBehaviour
    {
        [Header("Volume Settings")]
        [Range(0f, 1f)]
        [SerializeField] private float _sfxVolume = 1f;

        [Header("Volume spécifique")]
        [Range(0f, 1f)]
        [SerializeField] private float _crushVolume = 1f;

        [Header("Lab Audio Events")]
        [SerializeField] private AudioEventScriptableObject _crushSound;
        [SerializeField] private AudioEventScriptableObject _cutSound;
        [SerializeField] private AudioEventScriptableObject _stirSound;
        [SerializeField] private AudioEventScriptableObject _discardSound;
        [SerializeField] private AudioEventScriptableObject _dropInCauldronSound;
        [SerializeField] private AudioEventScriptableObject _cellarOpenSound;
        [SerializeField] private AudioEventScriptableObject _trapdoorUseSound;
        [SerializeField] private AudioEventScriptableObject _ingredientSelectTableSound;
        [SerializeField] private AudioEventScriptableObject _ingredientPlaceTableSound;
        [SerializeField] private AudioEventScriptableObject _qteSound;

        [Header("Speed Settings")]
        [SerializeField, Range(0.1f, 2f)] private float _cutSpeed = 1f;

        private EventInstance _crushInstance;
        private EventInstance _cutInstance;

        private void PlaySound(AudioEventScriptableObject audioEvent)
        {
            if (audioEvent == null || AudioManager.Instance == null) return;

            var instance = RuntimeManager.CreateInstance(audioEvent.EventReference);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
            instance.setVolume(_sfxVolume);
            instance.start();
            instance.release();
        }

        public void PlayCrushSound()
        {
            if (_crushSound == null || AudioManager.Instance == null) return;

            _crushInstance = RuntimeManager.CreateInstance(_crushSound.EventReference);
            _crushInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

            _crushInstance.setParameterByName("CrushVolume", _crushVolume);
            _crushInstance.start();
        }

        public void StopCrushSound()
        {
            if (_crushInstance.isValid())
            {
                _crushInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                _crushInstance.release();
                _crushInstance.clearHandle();
            }
        }

        public void UpdateCrushVolume(float newVolume)
        {
            _crushVolume = Mathf.Clamp01(newVolume);
            if (_crushInstance.isValid())
            {
                _crushInstance.setParameterByName("CrushVolume", _crushVolume);
            }
        }

        public void PlayCutSound()
        {
            if (_cutSound == null || AudioManager.Instance == null) return;

            _cutInstance = RuntimeManager.CreateInstance(_cutSound.EventReference);
            _cutInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
            _cutInstance.setVolume(_sfxVolume);
            _cutInstance.setParameterByName("CutSpeed", _cutSpeed);
            _cutInstance.start();
        }

        public void StopCutSound()
        {
            if (_cutInstance.isValid())
            {
                _cutInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                _cutInstance.release();
                _cutInstance.clearHandle();
            }
        }

        public void PlayStirSound() => PlaySound(_stirSound);
        public void PlayDiscardSound() => PlaySound(_discardSound);
        public void PlayDropInCauldronSound() => PlaySound(_dropInCauldronSound);
        public void PlayCellarOpenSound() => PlaySound(_cellarOpenSound);
        public void PlayTrapdoorUseSound() => PlaySound(_trapdoorUseSound);
        public void PlayIngredientSelectTableSound() => PlaySound(_ingredientSelectTableSound);
        public void PlayIngredientPlaceTableSound() => PlaySound(_ingredientPlaceTableSound);
        public void PlayQTESound() => PlaySound(_qteSound);

        public void SetSFXVolume(float value) => _sfxVolume = Mathf.Clamp01(value);
    }
}