using UnityEngine;

namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "LastSceneNameData", menuName = "Scriptable Objects/LastSceneNameData")]
    public class LastSceneNameData : ScriptableObject
    {
        public string sceneName; 
    }
}
