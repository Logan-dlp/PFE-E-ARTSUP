using UnityEngine;

public class SceneStarter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void Start()
    {
        SceneLoader.InitSceneLoader();
    }

    public void ChangeScene(string sceneName)
    {
        SceneLoader.LoadAsyncScene(sceneName, animator);
    }
}
