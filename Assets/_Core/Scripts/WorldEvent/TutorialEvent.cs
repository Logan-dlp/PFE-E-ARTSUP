using MoonlitMixes.Events;
using UnityEngine;

namespace MoonlitMixes.Tutorial
{
    public class TutorialEvent : MonoBehaviour
    {
        [SerializeField] private ScriptableEvent scriptableEvent;
        private void OnDisable()
        {
            scriptableEvent.SendEvent();
        }
    }
}
