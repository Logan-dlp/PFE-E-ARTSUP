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
        [SerializeField] private ScriptableEvent scriptableEvent;

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
                    break;
                case EnumScene.Labo:
                    break;
                default:
                    break;
            }
        }

        private void TutorialShopMorningPart1()
        {
            scriptableEvent.OnEvent += TutorialShopMorningPart3;
            FindFirstObjectByType<QuestBoard>().LoadQuestBoard();
            DialogueController.Instance.StartDialogue(dialogueDatasArray[0]);
        }

        private void TutorialShopMorningPart3()
        {
            scriptableEvent.OnEvent -= TutorialShopMorningPart3;
            _tutorialSaveInfo.tutorialShopMorningDone = true;
            _outlineToActivate[1].GetComponentInParent<PickUpTool>().canToolPickedUp = true;
            _outlineToActivate[1].SetActive(true);
        }

        private void TutorialHub()
        {

        }

        private void TutorialForest()
        {

        }

        private void TutorialLabo()
        {

        }
        
        private void TutorialShopTwilight()
        {
            
        }
    }
}
