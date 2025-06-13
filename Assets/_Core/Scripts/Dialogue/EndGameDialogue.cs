using MoonlitMixes.Datas;
using UnityEngine;

namespace MoonlitMixes.Dialogue
{

    public class EndGameDialogue : MonoBehaviour
    {
        [SerializeField] private Potion.PotionPriceCalculate _potionPriceCalculate;
        [SerializeField] private DialogueData _loseDialogue;
        [SerializeField] private DialogueData _winDialogue;
        private DialogueController _dialogueController;

        private void Start()
        {
            _dialogueController = gameObject.GetComponent<DialogueController>();
        }

        public void EndGame()
        {
            if (_potionPriceCalculate.IsLoanRefunded) //Win dialogue
            {
                _dialogueController.StartDialogue(_winDialogue);
            }
            else //lose dialogue
            {
                _dialogueController.StartDialogue(_loseDialogue);

            }
        }
    }

}

