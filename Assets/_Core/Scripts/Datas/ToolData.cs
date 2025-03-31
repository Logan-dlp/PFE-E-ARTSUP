using UnityEngine;

namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "Tool", menuName = "Scriptable Objects/Tool")]
    public class ToolData : ScriptableObject
    {
        [SerializeField] private GameObject _toolPrefab;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private string _obejectName;
        [SerializeField] private string _description;

        public string ObjectName
        {
            get => _obejectName;
        }

        public Sprite ItemSprite
        {
            get => _sprite;
        }

        public string Description
        {
            get => _description;
        }
    
        public GameObject ToolPrefab
        {
            get => _toolPrefab;
        }
    }
}