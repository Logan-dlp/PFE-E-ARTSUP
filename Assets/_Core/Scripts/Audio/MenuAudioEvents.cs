using FMODUnity;
using System;
using UnityEngine;

public class MenuAudioEvents : MonoBehaviour
{
    [Range(0f, 1f)][SerializeField] private float _sfxVolume = 1f;

    [SerializeField] private AudioEventScriptableObject _selectSound;

    private void PlaySound(AudioEventScriptableObject audioEvent)
    {
        if (audioEvent == null || AudioManager.Instance == null) return;

        var instance = RuntimeManager.CreateInstance(audioEvent.EventReference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        instance.setVolume(_sfxVolume);
        instance.start();
        instance.release();
    }

    public void PlaySelectSound()
    {
        PlaySound(_selectSound);
    }

    public void SetSFXVolume(float value)
    {
        _sfxVolume = Mathf.Clamp01(value);
    }
}