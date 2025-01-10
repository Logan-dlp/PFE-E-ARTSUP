using UnityEngine;

[CreateAssetMenu(fileName = "Tool", menuName = "Scriptable Objects/Tool")]
public class ToolData : ScriptableObject
{
    [SerializeField] private string _obejctName;
    [SerializeField] private GameObject _toolPrefab;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _description;

    public string ObjectName
    {
        get => _obejctName;
    }

    public Sprite ItemSprite
    {
        get => _sprite;
    }

    private string Description
    {
        get => _description;
    }
    private GameObject ToolPrefab
    {
        get => _toolPrefab;
    }
}
