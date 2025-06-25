using UnityEngine;

namespace MoonlitMixes.Tutorial
{
    [CreateAssetMenu(fileName = "TutorialSaveInfo", menuName = "Scriptable Objects/TutorialSaveInfo")]
    public class TutorialSaveInfo : ScriptableObject
    {
        public bool tutorialShopMorningDone;
        public bool tutorialShopMorningPostForestDone;
        public bool tutorialCaveDone;
        public bool tutorialForestDone;
        public bool tutorialPreCaveDone;
        public bool tutorialLabDone;
        public bool tutorialShopNightDone; 
    }
}
