using FMODUnity;
using UnityEngine;

public class PlayerShopAudioEvents : MonoBehaviour
{
    [Header("Volume Settings")]
    [Range(0f, 1f)][SerializeField] private float _sfxVolume = 1f;

    [Header("Audio Events")]
    [SerializeField] private AudioEventScriptableObject _clientBellSound;
    [SerializeField] private AudioEventScriptableObject _doorOpenSound;
    [SerializeField] private AudioEventScriptableObject _saleSuccessSound;
    [SerializeField] private AudioEventScriptableObject _saleFailedSound;
    [SerializeField] private AudioEventScriptableObject _dialogCloseSound;
    [SerializeField] private AudioEventScriptableObject _dialogSkipSound;
    [SerializeField] private AudioEventScriptableObject _selectPotionSound;
    [SerializeField] private AudioEventScriptableObject _npcMaleVoiceSound;
    [SerializeField] private AudioEventScriptableObject _npcFemaleVoiceSound;

    private void PlaySound(AudioEventScriptableObject audioEvent)
    {
        if (audioEvent == null || AudioManager.Instance == null) return;

        var instance = RuntimeManager.CreateInstance(audioEvent.EventReference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        instance.setVolume(_sfxVolume);
        instance.start();
        instance.release();
    }

    public void PlayClientBellSound() => PlaySound(_clientBellSound);
    public void PlayDoorOpenSound() => PlaySound(_doorOpenSound);
    public void PlaySaleSuccessSound() => PlaySound(_saleSuccessSound);
    public void PlaySaleFailedSound() => PlaySound(_saleFailedSound);
    public void PlayDialogCloseSound() => PlaySound(_dialogCloseSound);
    public void PlayDialogSkipSound() => PlaySound(_dialogSkipSound);
    public void PlaySelectPotionSound() => PlaySound(_selectPotionSound);
    public void PlayNpcMaleVoiceSound() => PlaySound(_npcMaleVoiceSound);
    public void PlayNpcFemaleVoiceSound() => PlaySound(_npcFemaleVoiceSound);

    public void SetSFXVolume(float value) => _sfxVolume = Mathf.Clamp01(value);
}
