using UnityEngine;

namespace MoonlitMixes.Tutorial
{
    [CreateAssetMenu(fileName = "TutorialSaveInfo", menuName = "Scriptable Objects/TutorialSaveInfo")]
    public class TutorialSaveInfo : ScriptableObject
    {
        public bool tutorialShopMorningDone;
        public bool tutorialHubDone;
        public bool tutorialForestDone;
        public bool tutorialLabDone;
        public bool tutorialShopNightDone; 
    }
}
