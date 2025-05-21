using System.Collections.Generic;
using UnityEngine;

namespace MoonlitMixes.SaveReference
{
    using Singleton;
    
    public class ScriptableSaveReference : PersistentMonoSingleton<ScriptableSaveReference>
    {
        [SerializeField] private List<ScriptableObject> _saveReferences;
    }
}