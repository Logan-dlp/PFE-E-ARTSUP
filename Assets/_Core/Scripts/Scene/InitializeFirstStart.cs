using MoonlitMixes.Datas;
using MoonlitMixes.Dialogue;
using UnityEngine;

namespace MoonlitMixes.Scene
{
    public class InitializeFirstStart : MonoBehaviour
    {
        [SerializeField] private LastSceneNameData _lastSceneNameData;
        [SerializeField] private DialogueData _dialogueData; 
        
        private void Start()
        {
            if (_lastSceneNameData.sceneName == "S_TitleScreen")
            {
                InitGame();
            }
        }

        private void InitGame()
        {
            FindFirstObjectByType<DialogueController>().StartDialogue(_dialogueData);
        }
    }
}
