public interface IAudioPlayer
{
    void Play(AudioEventScriptableObject audioEvent);
    void Stop(AudioEventScriptableObject audioEvent);
    void PlayPersistentAmbience(AudioEventScriptableObject audioEvent);
    void StopPersistentAmbience();
    void PlayPersistentMusic(AudioEventScriptableObject audioEvent);
    void StopPersistentMusic();
}