using UnityEngine;

namespace MoonlitMixes.UI
{
    public class ChapterLocker : MonoBehaviour
    {
        private void OnEnable()
        {
            ChangeChapterUI.CanChangeChapter = false;

        }

        private void OnDisable()
        {
            ChangeChapterUI.CanChangeChapter = true;
        }
    }
}
