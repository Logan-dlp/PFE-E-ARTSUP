using MoonlitMixes.Datas;
using UnityEngine;

namespace MoonlitMixes.Dialogue
{
    public class StartEndCinematic : MonoBehaviour
    {
        [SerializeField] private DialogueData dialogueData;

        void Start()
        {
            DialogueController.Instance.StartDialogue(dialogueData);
        }
    }
}
