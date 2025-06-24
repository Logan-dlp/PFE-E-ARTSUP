using UnityEngine;
using MoonlitMixes.Datas;
using UnityEngine.SceneManagement;

namespace MoonlitMixes.DayNightCycle
{
    public class OutlineActivatorInLabo : MonoBehaviour
    {
        [SerializeField] private GameObject[] outlinesToActivate;
        [SerializeField] private DayNightCycleInfo dayNightCycleInfo;

        private static bool hasActivatedInLabo = false;

        private void Start()
        {
            if (SceneManager.GetActiveScene().name == "S_Labo" && !hasActivatedInLabo)
            {
                if (dayNightCycleInfo.ActualTimePhase == 1)
                {
                    ActivateOutlines();
                    hasActivatedInLabo = true;
                }
            }
        }

        private void ActivateOutlines()
        {
            foreach (GameObject outline in outlinesToActivate)
            {
                if (outline != null)
                    outline.SetActive(true);
            }
        }

        public void DeactivateOutlines()
        {
            foreach (GameObject outline in outlinesToActivate)
            {
                if (outline != null)
                    outline.SetActive(false);
            }
        }
    }
}