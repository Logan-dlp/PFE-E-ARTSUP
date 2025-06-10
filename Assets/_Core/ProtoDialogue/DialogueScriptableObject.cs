using UnityEngine;

[CreateAssetMenu(fileName = "DialogueScriptableObject", menuName = "Scriptable Objects/DialogueScriptableObject")]
public class DialogueScriptableObject : ScriptableObject
{
    [System.Serializable]
    public struct DialogueSection
    {
        [TextArea]
        public string dialogue;
        public Sprite sprite;
        public bool isPlayer;
    }

    public DialogueSection[] dialogueSection;
}