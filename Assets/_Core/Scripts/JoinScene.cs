using MoonlitMixes.Scene;
using UnityEngine;

public class JoinScene2 : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;

    public void ChangeScene()
    {
        FindAnyObjectByType<SceneStarter>().ChangeScene(_sceneToLoad);
    }
}