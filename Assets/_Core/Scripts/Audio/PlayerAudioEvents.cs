using MoonlitMixes.Player;
using UnityEngine;

public class PlayerAudioEvents : MonoBehaviour
{
    [Header("Player Sounds")]
    public AudioEventScriptableObject _footstepSound;

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
        AudioManager.Instance.Play(_footstepSound);
    }
}