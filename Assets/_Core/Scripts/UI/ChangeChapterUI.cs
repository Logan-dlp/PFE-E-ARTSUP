using UnityEngine;
using UnityEngine.InputSystem;

namespace MoonlitMixes.UI
{
    public class ChangeChapterUI : MonoBehaviour
    {
        [SerializeField] private GameObject[] _chapterArray;
        [SerializeField] private ScriptableCallbackContextEvent _scriptableCallbackContextEvent;

        private int _chapterIndex;

        private void OnEnable()
        {
            _scriptableCallbackContextEvent.OnContextEvent += ChangeChapter;
        }

        private void OnDisable()
        {
            _scriptableCallbackContextEvent.OnContextEvent -= ChangeChapter;
        }
        
        public void ChangeChapter(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _chapterArray[_chapterIndex].SetActive(false);

                if (context.ReadValue<float>() > 0)
                {
                    if (_chapterIndex < _chapterArray.Length - 1)
                    {
                        _chapterIndex++;

                    }
                    else
                    {
                        _chapterIndex = 0;
                    }
                }
                else
                {
                    if (_chapterIndex > 0)
                    {
                        _chapterIndex--;
                    }
                    else
                    {
                        _chapterIndex = _chapterArray.Length - 1;
                    }
                }

                _chapterArray[_chapterIndex].SetActive(true);
            }
        }
    }
}
