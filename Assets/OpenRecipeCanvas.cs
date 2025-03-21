using UnityEngine;

public class OpenRecipeCanvas : MonoBehaviour
{
    public void OpenCloseCanvas()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
