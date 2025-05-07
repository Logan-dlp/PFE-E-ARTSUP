using MoonlitMixes.Datas;
using UnityEngine;

namespace MoonlitMixes.Scene
{
    public class PlayerPlacement : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private LastSceneNameData _lastSceneNameData;
        [SerializeField] private SpawnPosition[] _spawnPositionsArray;

        private void Awake()
        {
            CharacterController characterController = _player.GetComponent<CharacterController>();

            foreach (var spawnPosition in _spawnPositionsArray)
            {
                if(spawnPosition.sceneNameLinked == _lastSceneNameData.sceneName)
                {
                    characterController.enabled = false;
                    _player.transform.position = spawnPosition.transform.position;
                    characterController.enabled = true;
                    break;
                }
            }
        }
    }
}
