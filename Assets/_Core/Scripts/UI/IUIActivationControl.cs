using UnityEngine;

public interface IUIActivationControl
{
    public Animator AnimatorUI { get; }
    public GameObject Panel { get; }

    public void OpenCanvas();
    public void OpenCanvas(string sceneName);
    public void CloseCanvas();
}
