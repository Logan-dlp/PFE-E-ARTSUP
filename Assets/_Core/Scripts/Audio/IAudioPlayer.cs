public interface IAudioPlayer
{
    void Play(AudioEventScriptableObject audioEvent);
    void Stop(AudioEventScriptableObject audioEvent);
    void PlayPersistentAmbience(AudioEventScriptableObject audioEvent, float volume);
    void PlayPersistentMusic(AudioEventScriptableObject audioEvent, float volume);
    void SetAmbienceVolume(float volume);
    void SetMusicVolume(float volume);
}