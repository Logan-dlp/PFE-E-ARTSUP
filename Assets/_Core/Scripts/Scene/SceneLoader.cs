using System.Collections;
using MoonlitMixes.Animation;
using MoonlitMixes.StaticCoroutines;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonlitMixes.Scene
{
    public static class SceneLoader
    {
        private static AsyncOperation _sceneToLoad;
        private static bool _animFinished;
       
        public static void LoadAsyncScene(string sceneName, Animator animator)
        {
            _animFinished = false;
            EndTransitionEvent.OnAnimEndAction += OnAnimEnd;
            _sceneToLoad = SceneManager.LoadSceneAsync(sceneName);
            _sceneToLoad.allowSceneActivation = false;
            animator.SetTrigger("Start");
            StaticCoroutine.Start(LoadingScene());
        }

        private static IEnumerator LoadingScene()
        {
            while(!_sceneToLoad.isDone)
            {
                if(_animFinished)
                {
                    _sceneToLoad.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        private static void OnAnimEnd()
        {
            _animFinished = true;
            EndTransitionEvent.OnAnimEndAction -= OnAnimEnd;
        }
    }
}