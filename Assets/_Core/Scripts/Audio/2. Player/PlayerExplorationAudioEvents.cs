using FMODUnity;
using MoonlitMixes.Health;
using MoonlitMixes.Player;
using UnityEngine;

public class PlayerExplorationAudioEvents : MonoBehaviour
{
    [Header("Audio Events")]
    [SerializeField] private AudioEventScriptableObject _lowHealthSound;
    [SerializeField] private AudioEventScriptableObject _lowStaminaSound;
    [SerializeField] private AudioEventScriptableObject _pickupItemSound;
    [SerializeField] private AudioEventScriptableObject _toolChangeSound;
    [SerializeField] private AudioEventScriptableObject _toolMacheteImpactSound;
    [SerializeField] private AudioEventScriptableObject _toolPickaxeImpactSound;
    [SerializeField] private AudioEventScriptableObject _toolStaffImpactSound;
    [SerializeField] private AudioEventScriptableObject _deathSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)][SerializeField] private float _defaultVolume = 1f;

    private PlayerHealth _playerHealth;

    private void PlaySound(AudioEventScriptableObject audioEvent, float volume = -1f)
    {
        if (audioEvent == null || AudioManager.Instance == null) return;

        var instance = RuntimeManager.CreateInstance(audioEvent.EventReference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        instance.setVolume(volume >= 0f ? volume : _defaultVolume);
        instance.start();
        instance.release();
    }

    private FMOD.Studio.EventInstance _lowHealthInstance;

    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        if (_playerHealth != null)
        {
            _playerHealth.OnLowHealth += StartLowHealthSound;
            _playerHealth.OnHealthRecovered += StopLowHealthSound;
        }
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.OnLowHealth -= StartLowHealthSound;
            _playerHealth.OnHealthRecovered -= StopLowHealthSound;
        }
    }

    private void StartLowHealthSound()
    {
        if (_lowHealthSound == null || AudioManager.Instance == null) return;

        if (_lowHealthInstance.isValid())
            _lowHealthInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        _lowHealthInstance = RuntimeManager.CreateInstance(_lowHealthSound.EventReference);
        _lowHealthInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        _lowHealthInstance.setVolume(_defaultVolume);
        _lowHealthInstance.start();
    }

    private void StopLowHealthSound()
    {
        if (_lowHealthInstance.isValid())
        {
            _lowHealthInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _lowHealthInstance.release();
        }
    }

    public void PlayLowStaminaSound() => PlaySound(_lowStaminaSound);

    public void PlayPickupItemSound() => PlaySound(_pickupItemSound);

    public void PlayToolChangeSound() => PlaySound(_toolChangeSound);

    public void PlayToolMacheteImpactSound() => PlaySound(_toolMacheteImpactSound);

    public void PlayToolPickaxeImpactSound() => PlaySound(_toolPickaxeImpactSound);

    public void PlayToolStaffImpactSound() => PlaySound(_toolStaffImpactSound);

    public void PlayDeathSound() => PlaySound(_deathSound);
}