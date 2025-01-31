using MoonlitMixes.Health;
using MoonlitMixes.Respawn;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawnInOtherScene : MonoBehaviour
{
    [SerializeField] private RespawnPointData _respawnPointData;

    private PlayerHealth _playerHealth;

    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        _playerHealth.OnPlayerRespawnInOtherScene += LoadRespawnScene;
    }

    private void OnDisable()
    {
        _playerHealth.OnPlayerRespawnInOtherScene -= LoadRespawnScene;
    }

    private void LoadRespawnScene()
    {
        SceneManager.LoadScene(_respawnPointData.RespawnScene);
        
        CharacterController player = GameObject.FindAnyObjectByType<CharacterController>();

        if (player != null)
        {
            if (player != null)
            {
                player.enabled = false;
            }
            player.transform.position = _respawnPointData.RespawnPosition;
            player.transform.rotation = _respawnPointData.RespawnRotation;

            
            if (player != null)
            {
                player.enabled = true;
            }
        }
    }
}
