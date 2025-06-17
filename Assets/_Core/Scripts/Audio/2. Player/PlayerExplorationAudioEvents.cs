using FMODUnity;
using MoonlitMixes.Health;
using MoonlitMixes.Interactions;
using MoonlitMixes.Player;
using System.Collections;
using UnityEngine;

public class PlayerExplorationAudioEvents : MonoBehaviour
{
    [Header("Volume Settings")]
    [Range(0f, 1f)][SerializeField] private float _sfxVolume = 1f;

    [Header("Audio Settings")]
    [SerializeField, Tooltip("Interval between low health sound loops in seconds.")]
    private float _lowHealthInterval = 2f;

    [SerializeField, Tooltip("Time in seconds for the low stamina sound to reach full volume.")]
    private float _lowStaminaFadeInTime = 2f;

    [Header("Audio Events")]
    [SerializeField] private AudioEventScriptableObject _lowHealthSound;
    [SerializeField] private AudioEventScriptableObject _lowStaminaSound;
    [SerializeField] private AudioEventScriptableObject _pickupItemSound;
    [SerializeField] private AudioEventScriptableObject _toolChangeSound;
    [SerializeField] private AudioEventScriptableObject _toolMacheteImpactSound;
    [SerializeField] private AudioEventScriptableObject _toolPickaxeImpactSound;
    [SerializeField] private AudioEventScriptableObject _toolStaffImpactSound;
    [SerializeField] private AudioEventScriptableObject _toolSepterSwing;
    [SerializeField] private AudioEventScriptableObject _deathSound;

    private PlayerHealth _playerHealth;
    private PlayerMovement _playerMovement;

    private FMOD.Studio.EventInstance _lowStaminaInstance;
    private Coroutine _lowHealthCoroutine;
    private Coroutine _lowStaminaCoroutine;

    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        if (_playerHealth != null)
        {
            _playerHealth.OnLowHealth += StartLowHealthSound;
            _playerHealth.OnHealthRecovered += StopLowHealthSound;
            _playerHealth.OnDeath += PlayDeathSound;
        }

        _playerMovement = GetComponent<PlayerMovement>();
        if (_playerMovement != null)
        {
            _playerMovement.OnLowStamina += StartLowStaminaSound;
            _playerMovement.OnStaminaRecovered += StopLowStaminaSound;
        }

        InteractionTools.OnUsedHand += PlayPickupItemSound;
        UseTools.OnUsedMachete += PlayToolMacheteImpactSound;
        UseTools.OnUsedPickaxe += PlayToolPickaxeImpactSound;
        UseTools.OnUsedSepter += PlayToolStaffImpactSound;
        UseTools.OnUsedSepterSwing += PlayToolSepterSwingSound;
        RouletteSelectionTools.OnToolChanged += PlayToolChangeSound;
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnLowHealth -= StartLowHealthSound;
            _playerHealth.OnHealthRecovered -= StopLowHealthSound;
            _playerHealth.OnDeath -= PlayDeathSound;
        }

        if (_playerMovement != null)
        {
            _playerMovement.OnLowStamina -= StartLowStaminaSound;
            _playerMovement.OnStaminaRecovered -= StopLowStaminaSound;
        }

        InteractionTools.OnUsedHand -= PlayPickupItemSound;
        UseTools.OnUsedMachete -= PlayToolMacheteImpactSound;
        UseTools.OnUsedPickaxe -= PlayToolPickaxeImpactSound;
        UseTools.OnUsedSepter -= PlayToolStaffImpactSound;
        UseTools.OnUsedSepterSwing -= PlayToolSepterSwingSound;
        RouletteSelectionTools.OnToolChanged -= PlayToolChangeSound;
    }

    private void PlaySound(AudioEventScriptableObject audioEvent)
    {
        if (audioEvent == null || AudioManager.Instance == null) return;

        var instance = RuntimeManager.CreateInstance(audioEvent.EventReference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        instance.setVolume(_sfxVolume);
        instance.start();
        instance.release();
    }

    private void StartLowHealthSound()
    {
        if (_lowHealthCoroutine == null)
            _lowHealthCoroutine = StartCoroutine(PlayLowHealthLoop());
    }

    private void StopLowHealthSound()
    {
        if (_lowHealthCoroutine != null)
        {
            StopCoroutine(_lowHealthCoroutine);
            _lowHealthCoroutine = null;
        }
    }

    private IEnumerator PlayLowHealthLoop()
    {
        while (true)
        {
            PlaySound(_lowHealthSound);
            yield return new WaitForSeconds(_lowHealthInterval);
        }
    }

    private void StartLowStaminaSound()
    {
        if (_lowStaminaSound == null || AudioManager.Instance == null) return;

        if (_lowStaminaInstance.isValid())
            _lowStaminaInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        _lowStaminaInstance = RuntimeManager.CreateInstance(_lowStaminaSound.EventReference);
        _lowStaminaInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        _lowStaminaInstance.setVolume(0f);
        _lowStaminaInstance.start();

        if (_lowStaminaCoroutine != null)
            StopCoroutine(_lowStaminaCoroutine);

        _lowStaminaCoroutine = StartCoroutine(FadeInLowStaminaVolume());
    }

    private IEnumerator FadeInLowStaminaVolume()
    {
        float timer = 0f;
        while (timer < _lowStaminaFadeInTime)
        {
            timer += Time.deltaTime;
            float volume = Mathf.Lerp(0f, _sfxVolume, timer / _lowStaminaFadeInTime);
            if (_lowStaminaInstance.isValid())
                _lowStaminaInstance.setVolume(volume);
            yield return null;
        }

        if (_lowStaminaInstance.isValid())
            _lowStaminaInstance.setVolume(_sfxVolume);
    }

    private void StopLowStaminaSound()
    {
        if (_lowStaminaCoroutine != null)
        {
            StopCoroutine(_lowStaminaCoroutine);
            _lowStaminaCoroutine = null;
        }

        if (_lowStaminaInstance.isValid())
        {
            _lowStaminaInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _lowStaminaInstance.release();
        }
    }

    public void PlayPickupItemSound() => PlaySound(_pickupItemSound);
    public void PlayToolChangeSound() => PlaySound(_toolChangeSound);
    public void PlayToolMacheteImpactSound() => PlaySound(_toolMacheteImpactSound);
    public void PlayToolPickaxeImpactSound() => PlaySound(_toolPickaxeImpactSound);
    public void PlayToolStaffImpactSound() => PlaySound(_toolStaffImpactSound);
    public void PlayToolSepterSwingSound() => PlaySound(_toolSepterSwing);
    public void PlayDeathSound() => PlaySound(_deathSound);

    // For maybe options
    public void SetSFXVolume(float value) => _sfxVolume = Mathf.Clamp01(value);
}