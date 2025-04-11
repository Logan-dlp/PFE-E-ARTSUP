using UnityEngine;
using System.Collections.Generic;

namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "ToolList", menuName = "Scriptable Objects/ToolList")]
    public class ToolListData : ScriptableObject
    {
        [SerializeField] private List<ToolData> _ToolListData;
        public List<ToolData> ToolList => _ToolListData; 
    }
}