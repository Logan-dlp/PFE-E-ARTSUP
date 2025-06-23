using System;
using System.Collections.Generic;
using MoonlitMixes.Datas;
using UnityEngine;

namespace MoonlitMixes.ExplorationTools
{
    [CreateAssetMenu(fileName = "ToolAcquired", menuName = "Scriptable Objects/ToolAcquired")]
    public class ToolAcquired : ScriptableObject
    {
        public List<Tool> toolAcquiredArray;

        [Serializable]
        public struct Tool
        {
            public GameObject toolPrefab;
            public ToolData toolData;
        }
    }
}
