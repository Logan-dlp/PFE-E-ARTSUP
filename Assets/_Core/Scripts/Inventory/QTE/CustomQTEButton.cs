using UnityEngine;

namespace MoonlitMixes.Datas.QTE
{
    public class CustomQTEButton
    {
        public InputCommand inputCommand;
        [Min(1)] public int requiredInput;
        [Min(1)] public float qTEDuration;
        public bool isProgressBar;
    }
}