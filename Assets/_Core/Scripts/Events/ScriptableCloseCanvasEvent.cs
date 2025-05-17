using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableCloseCanvasEvent", menuName = "Scriptable Objects/Event/ScriptableCloseCanvasEvent")]
public class ScriptableCloseCanvasEvent : ScriptableObject
{
    public event Action CloseCanvasAction;
    
    public void CloseCanvas()
    {
        CloseCanvasAction?.Invoke();
    }
}
