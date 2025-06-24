using System;
using MoonlitMixes.Datas;
using MoonlitMixes.Dialogue;
using MoonlitMixes.Events;
using MoonlitMixes.ExplorationTools;
using MoonlitMixes.Quest;
using UnityEngine;

namespace MoonlitMixes.Tutorial
{
    public class TutorialActivation : MonoBehaviour
    {
        [SerializeField] private LastSceneNameData _lastSceneNameData;
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;
        [SerializeField] private TutorialSaveInfo _tutorialSaveInfo;
        [SerializeField] private EnumScene enumScene;
        [SerializeField] private ScriptableEvent scriptableEvent1;
        [SerializeField] private ScriptableEvent scriptableEvent2;

        [SerializeField] private DialogueData[] dialogueDatasArray;
        [SerializeField] private GameObject[] _outlineToActivate;

        private void Start()
        {
            switch (enumScene)
            {
                case EnumScene.ShopMorning:
                    if (!_tutorialSaveInfo.tutorialHubDone && _lastSceneNameData.sceneName == "S_TitleScreen")
                    {
                        TutorialShopMorningPart1();
                    }
                    break;
                case EnumScene.Forest:
                    if (!_tutorialSaveInfo.tutorialHubDone)
                    {
                        TutorialForestPart1();
                    }
                    break;
                case EnumScene.Labo:
                    break;
                default:
                    break;
            }
        }

        private void TutorialShopMorningPart1()
        {
            scriptableEvent1.OnEvent += TutorialShopMorningPart2;
            FindFirstObjectByType<QuestBoard>().LoadQuestBoard();
            DialogueController.Instance.StartDialogue(dialogueDatasArray[0]);
        }

        private void TutorialShopMorningPart2()
        {
            scriptableEvent1.OnEvent -= TutorialShopMorningPart2;
            scriptableEvent2.OnEvent += TutorialShopMorningPart3;
            DialogueController.Instance.StartDialogue(dialogueDatasArray[1]);
            _outlineToActivate[1].GetComponentInParent<PickUpTool>().canToolPickedUp = true;
            _outlineToActivate[1].SetActive(true);
        }

        private void TutorialShopMorningPart3()
        {
            scriptableEvent2.OnEvent -= TutorialShopMorningPart3;
            QuestBoard._hasQuestBeenSendToday = true;
            DialogueController.Instance.StartDialogue(dialogueDatasArray[2]);
            _tutorialSaveInfo.tutorialShopMorningDone = true;
        }

        private void TutorialForestPart1()
        {
            _outlineToActivate[0].SetActive(true);
            scriptableEvent2.OnEvent += TutorialForestPart2;
            DialogueController.Instance.StartDialogue(dialogueDatasArray[0]);
        }

        private void TutorialForestPart2()
        {
            _outlineToActivate[0].SetActive(false);
            _outlineToActivate[1].SetActive(true);
            _outlineToActivate[1].GetComponentInParent<PickUpTool>().canToolPickedUp = true;
            scriptableEvent2.OnEvent -= TutorialForestPart2;
            DialogueController.Instance.StartDialogue(dialogueDatasArray[1]);
            _tutorialSaveInfo.tutorialForestDone = true;
        }

        private void TutorialLabo()
        {

        }
        
        private void TutorialShopTwilight()
        {
            
        }
    }
}
