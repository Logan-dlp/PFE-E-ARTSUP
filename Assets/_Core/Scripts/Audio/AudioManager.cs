using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IAudioPlayer
{
    private static AudioManager _instance;
    public static IAudioPlayer Instance => _instance;

    private Dictionary<AudioEventScriptableObject, EventInstance> _activeEvents = new();
    private EventInstance? _persistentAmbience;
    private AudioEventScriptableObject _currentAmbience;
    private EventInstance? _persistentMusic;
    private AudioEventScriptableObject _currentMusic;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioEventScriptableObject audioEvent)
    {
        if (audioEvent == null) return;

        EventInstance instance = RuntimeManager.CreateInstance(audioEvent.EventReference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        instance.start();
        instance.release();
        _activeEvents[audioEvent] = instance;
    }

    public void Stop(AudioEventScriptableObject audioEvent)
    {
        if (audioEvent == null) return;

        if (!_activeEvents.TryGetValue(audioEvent, out EventInstance instance)) return;

        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
        _activeEvents.Remove(audioEvent);
    }

    public void PlayPersistentAmbience(AudioEventScriptableObject audioEvent, float volume = 1f)
    {
        if (audioEvent == null) return;

        StopPersistentAmbience();

        EventInstance instance = RuntimeManager.CreateInstance(audioEvent.EventReference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        instance.setVolume(Mathf.Clamp01(volume));
        instance.start();

        _persistentAmbience = instance;
        _currentAmbience = audioEvent;
    }

    public void PlayPersistentMusic(AudioEventScriptableObject audioEvent, float volume = 1f)
    {
        if (audioEvent == null) return;

        StopPersistentMusic();

        EventInstance instance = RuntimeManager.CreateInstance(audioEvent.EventReference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        instance.setVolume(Mathf.Clamp01(volume));
        instance.start();

        _persistentMusic = instance;
        _currentMusic = audioEvent;
    }

    public void SetAmbienceVolume(float volume)
    {
        if (_persistentAmbience.HasValue)
        {
            _persistentAmbience.Value.setVolume(Mathf.Clamp01(volume));
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (_persistentMusic.HasValue)
        {
            _persistentMusic.Value.setVolume(Mathf.Clamp01(volume));
        }
    }

    private void StopPersistentAmbience()
    {
        if (_persistentAmbience.HasValue)
        {
            _persistentAmbience.Value.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _persistentAmbience.Value.release();
            _persistentAmbience = null;
            _currentAmbience = null;
        }
    }

    private void StopPersistentMusic()
    {
        if (_persistentMusic.HasValue)
        {
            _persistentMusic.Value.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _persistentMusic.Value.release();
            _persistentMusic = null;
            _currentMusic = null;
        }
    }
}