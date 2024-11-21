using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogueScriptable;
    [SerializeField] private TMP_Text _textPlayer;
    [SerializeField] private TMP_Text _textNPC;
    [SerializeField] private Image _imagePlayer;
    [SerializeField] private Image _imageNPC;

    private int dialogueIndex = 0;

    public void NextDialogue()
    {
        if(dialogueIndex != dialogueScriptable.dialogueSection.Length)
        {
            if(dialogueScriptable.dialogueSection[dialogueIndex].isPlayer)
            {
                _textPlayer.text = dialogueScriptable.dialogueSection[dialogueIndex].dialogue;
                _imagePlayer.sprite = dialogueScriptable.dialogueSection[dialogueIndex].sprite;
                _imagePlayer.preserveAspect = true;
                dialogueIndex++;
            }
            else
            {
                _textNPC.text = dialogueScriptable.dialogueSection[dialogueIndex].dialogue;
                _imageNPC.sprite = dialogueScriptable.dialogueSection[dialogueIndex].sprite;
                _imageNPC.preserveAspect = true;
                dialogueIndex++;
            }
        }
        else if(dialogueIndex == dialogueScriptable.dialogueSection.Length)
        {

        }
    } 
}
