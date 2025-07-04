using MoonlitMixes.AI.PNJ;
using MoonlitMixes.Datas;
using MoonlitMixes.DayNightCycle;
using MoonlitMixes.Dialogue;
using MoonlitMixes.Events;
using MoonlitMixes.ExplorationTools;
using MoonlitMixes.Quest;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MoonlitMixes.Tutorial
{
    public class TutorialActivation : MonoBehaviour
    {
        [SerializeField] private LastSceneNameData _lastSceneNameData;
        [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;
        [SerializeField] private TutorialSaveInfo _tutorialSaveInfo;
        [SerializeField] private ToolAcquired _toolAcquired;
        [SerializeField] private EnumScene enumScene;
        [SerializeField] private ScriptableEvent scriptableEvent1;
        [SerializeField] private ScriptableEvent scriptableEvent2;

        [SerializeField] private DialogueData[] dialogueDatasArray;
        [SerializeField] private GameObject[] _outlineToActivate;
        [SerializeField] private MoneyData _moneyData;

        private void Start()
        {
            if (_lastSceneNameData.sceneName == "S_TitleScreen")
            {
                ResetTutorialSaveInfo();
            }

            switch (enumScene)
            {
                case EnumScene.ShopMorning:
                    if (!_tutorialSaveInfo.tutorialShopMorningDone && _lastSceneNameData.sceneName == "S_TitleScreen")
                    {
                        TutorialShopMorningDay1Part1();
                        return;
                    }
                    else if (!_tutorialSaveInfo.tutorialShopMorningPostForestDone && _dayNightCycleInfo.ActualTimePhase == (int)EnumDayPhase.Afternoon)
                    {
                        TutorialShopMorningDay1Part4();
                        return;
                    }
                    else if (!_tutorialSaveInfo.tutorialPreCaveDone && _dayNightCycleInfo.ActualDay == 1)
                    {
                        TutorialPreCavePart1();
                        return;
                    }
                    break;
                case EnumScene.Forest:
                    if (!_tutorialSaveInfo.tutorialForestDone)
                    {
                        TutorialForestPart1();
                        return;
                    }
                    break;
                case EnumScene.Labo:
                    break;
                case EnumScene.ShopTwilight:
                    if (!_tutorialSaveInfo.tutorialShopNightDone)
                    {
                        TutorialShopTwilightPart1();
                    }
                    break;
                default:
                    if (!_tutorialSaveInfo.tutorialCaveDone)
                    {
                        TutorialCavePart1();
                    }
                    break;
            }
        }

        private void ResetTutorialSaveInfo()
        {
            _tutorialSaveInfo.tutorialShopMorningDone = false;
            _tutorialSaveInfo.tutorialShopMorningPostForestDone = false;
            _tutorialSaveInfo.tutorialCaveDone = false;
            _tutorialSaveInfo.tutorialForestDone = false;
            _tutorialSaveInfo.tutorialPreCaveDone = false;
            _tutorialSaveInfo.tutorialLabDone = false;
            _tutorialSaveInfo.tutorialShopNightDone = false;

            _toolAcquired.toolAcquiredArray.Clear();
        }

        private void TutorialShopMorningDay1Part1()
        {
            _outlineToActivate[0].transform.parent.gameObject.SetActive(true);
            _outlineToActivate[0].SetActive(true);
            scriptableEvent1.OnEvent += TutorialShopMorningDay1Part2;
            FindFirstObjectByType<QuestBoard>().LoadQuestBoard();
            DialogueController.Instance.StartDialogue(dialogueDatasArray[0]);
        }

        private void TutorialShopMorningDay1Part2()
        {
            _outlineToActivate[0].SetActive(false);
            scriptableEvent1.OnEvent -= TutorialShopMorningDay1Part2;
            scriptableEvent2.OnEvent += TutorialShopMorningDay1Part3;
            DialogueController.Instance.StartDialogue(dialogueDatasArray[1]);
            _outlineToActivate[1].GetComponentInParent<PickUpTool>().canToolPickedUp = true;
            _outlineToActivate[1].SetActive(true);
        }

        private void TutorialShopMorningDay1Part3()
        {
            scriptableEvent2.OnEvent -= TutorialShopMorningDay1Part3;
            QuestBoard._hasQuestBeenSendToday = true;
            DialogueController.Instance.StartDialogue(dialogueDatasArray[2]);
            _tutorialSaveInfo.tutorialShopMorningDone = true;
        }

        private void TutorialShopMorningDay1Part4()
        {
            _outlineToActivate[2].SetActive(true);
            DialogueController.Instance.StartDialogue(dialogueDatasArray[3]);
            _tutorialSaveInfo.tutorialShopMorningPostForestDone = true;
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

        private void TutorialPreCavePart1()
        {
            _tutorialSaveInfo.tutorialPreCaveDone = true;
            DialogueController.Instance.StartDialogue(dialogueDatasArray[4]);
        }

        private void TutorialLabo()
        {

        }

        private void TutorialShopTwilightPart1()
        {
            _outlineToActivate[0].SetActive(true);
            DialogueController.Instance.StartDialogue(dialogueDatasArray[0]);
        }

        public void TutorialShopTwilightPart2()
        {
            _tutorialSaveInfo.tutorialShopNightDone = true;
            DialogueController.Instance.StartDialogue(dialogueDatasArray[1]);
            scriptableEvent1.OnEvent += TutorialShopTwilightPart3;
        }

        private void TutorialShopTwilightPart3()
        {
            _outlineToActivate[0].SetActive(false);
            scriptableEvent1.OnEvent -= TutorialShopTwilightPart3;
            FindFirstObjectByType<CloseOrOpenShop>().OnToggleShop();
        }

        public void TutorialShopTwilightPart4()
        {
            _outlineToActivate[1].SetActive(true);

            if (_moneyData.money < 120)
            {
                DialogueController.Instance.StartDialogue(dialogueDatasArray[3]);
            }
            else
            {
                DialogueController.Instance.StartDialogue(dialogueDatasArray[2]);
            }
        }

        private void TutorialCavePart1()
        {
            scriptableEvent1.OnEvent += TutorialCavePart2;
            _outlineToActivate[0].SetActive(true);
            DialogueController.Instance.StartDialogue(dialogueDatasArray[0]);
            _outlineToActivate[0].GetComponentInParent<PickUpTool>().canToolPickedUp = true;
        }

        private void TutorialCavePart2()
        {
            scriptableEvent1.OnEvent -= TutorialCavePart2;
            DialogueController.Instance.StartDialogue(dialogueDatasArray[1]);
            _tutorialSaveInfo.tutorialCaveDone = true;
        }
    }
}