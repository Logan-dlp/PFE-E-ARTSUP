using FMODUnity;
using MoonlitMixes.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAudioEvents : MonoBehaviour
{
    [Header("Footsteps sound parameters")]
    [Range(0f, 1f)]
    [SerializeField] private float _footstepsVolume = 1f;

    [SerializeField] private AudioEventScriptableObject _footstepsSound;

    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        if (_playerMovement != null)
        {
            _playerMovement.OnFootstep += PlayFootstep;
        }
    }

    private void OnDestroy()
    {
        if (_playerMovement != null)
        {
            _playerMovement.OnFootstep -= PlayFootstep;
        }
    }

    public void PlayFootstep()
    {
        if (_footstepsSound == null || AudioManager.Instance == null) return;

        var instance = FMODUnity.RuntimeManager.CreateInstance(_footstepsSound.EventReference);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        instance.setVolume(_footstepsVolume);

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "S_Labo")
        {
            RuntimeManager.StudioSystem.setParameterByName("FootstepsLocalisation", 1f);
        }
        else if (currentSceneName == "S_Shop_Morning" || currentSceneName == "S_Shop_Twilight")
        {
            RuntimeManager.StudioSystem.setParameterByName("FootstepsLocalisation", 2f);
        }

        instance.start();
        instance.release();
    }
}