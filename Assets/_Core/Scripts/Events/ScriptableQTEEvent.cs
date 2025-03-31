using System;
using MoonlitMixes.Datas;
using MoonlitMixes.Datas.QTE;
using UnityEngine;

namespace MoonlitMixes.Events
{
    [CreateAssetMenu(fileName = "ScriptableQTEEvent", menuName = "Scriptable Objects/ScriptableQTEEvent")]
    public class ScriptableQTEEvent : ScriptableObject
    {
        public event Action<ScriptableQTEConfig> ScriptableQTEConfigAction;
        
        public void SendObject(ScriptableQTEConfig scriptableQTEConfig)
        {
            ScriptableQTEConfigAction?.Invoke(scriptableQTEConfig);
        }
    }
}