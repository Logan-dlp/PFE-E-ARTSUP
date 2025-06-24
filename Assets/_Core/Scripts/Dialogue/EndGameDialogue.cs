using MoonlitMixes.Datas;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

namespace MoonlitMixes.Dialogue
{

    public class EndGameDialogue : MonoBehaviour
    {
        [SerializeField] private Potion.PotionPriceCalculate _potionPriceCalculate;
        [SerializeField] private DialogueData _dailyLoseDialogue;
        [SerializeField] private DialogueData _dailyWinDialogue;
        [SerializeField] private DialogueData _loseDialogue;
        [SerializeField] private DialogueData _winDialogue;
        [SerializeField] private GameObject _endImage;
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;
        private DialogueController _dialogueController;

        private void Start()
        {
            _dialogueController = gameObject.GetComponent<DialogueController>();
        }
        [Button]
        public void EndDayDialogue()
        {
            switch (_dayNightCycleInfo.ActualDay)
            {
                case 0:
                    if(_potionPriceCalculate.Day1Money>=_potionPriceCalculate.Day1NeededMoney) _dialogueController.StartDialogue(_dailyWinDialogue);
                    else _dialogueController.StartDialogue(_dailyLoseDialogue);
                    break;
                case 1:
                    if (_potionPriceCalculate.Day2Money >= _potionPriceCalculate.Day2NeededMoney) _dialogueController.StartDialogue(_dailyWinDialogue);
                    else _dialogueController.StartDialogue(_dailyLoseDialogue);
                    break;
                case 2:
                    _dialogueController.IsLastDialogue = true;
                    if (_potionPriceCalculate.IsLoanRefunded) _dialogueController.StartDialogue(_winDialogue);
                    else _dialogueController.StartDialogue(_loseDialogue);
                    break;
                default:
                    Debug.LogWarning("Mauvaise valeure de jour envoyé");
                    break;
            }
        }
        public void EndGame()
        {
            if(_endImage != null) _endImage.SetActive(true);
        }
        public IEnumerator DelayGame() 
        { 
            yield return new WaitForSeconds(5);
        }
    }
}

