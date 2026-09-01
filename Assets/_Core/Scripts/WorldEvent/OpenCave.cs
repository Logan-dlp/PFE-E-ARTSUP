using MoonlitMixes.Tutorial;
using UnityEngine;

namespace MoonlitMixes.Scene
{
    public class OpenCave : MonoBehaviour
    {
        [SerializeField] private GameObject _rocksToActivate;
        [SerializeField] private TutorialSaveInfo _tutorialSaveInfo;

        private void Start()
        {
            RemoveRocks();
        }

        public void RemoveRocks()
        {
            if (_tutorialSaveInfo.tutorialPreCaveDone)
            {
                _rocksToActivate.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
