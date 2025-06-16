using UnityEngine;
using FMODUnity;
using MoonlitMixes.Player;

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

    private void PlaySound(AudioEventScriptableObject audioEvent, float volume = -1f)
    {
        if (audioEvent == null || AudioManager.Instance == null) return;

        var instance = RuntimeManager.CreateInstance(audioEvent.EventReference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        instance.setVolume(volume >= 0f ? volume : _defaultVolume);
        instance.start();
        instance.release();
    }


    public void PlayLowHealthSound() => PlaySound(_lowHealthSound);

    public void PlayLowStaminaSound() => PlaySound(_lowStaminaSound);

    public void PlayPickupItemSound() => PlaySound(_pickupItemSound);

    public void PlayToolChangeSound() => PlaySound(_toolChangeSound);

    public void PlayToolMacheteImpactSound() => PlaySound(_toolMacheteImpactSound);

    public void PlayToolPickaxeImpactSound() => PlaySound(_toolPickaxeImpactSound);

    public void PlayToolStaffImpactSound() => PlaySound(_toolStaffImpactSound);

    public void PlayDeathSound() => PlaySound(_deathSound);
}