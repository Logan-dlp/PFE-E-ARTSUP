using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private static List<AsyncOperation> _sceneToLoad = new List<AsyncOperation>();
    public static void LoadAsyncScene(string sceneName)
    {
        _sceneToLoad.Add(SceneManager.LoadSceneAsync(sceneName));
    }
}
