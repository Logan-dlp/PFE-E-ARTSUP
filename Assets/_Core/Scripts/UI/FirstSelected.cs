using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSelected : MonoBehaviour
{
    [SerializeField] private GameObject _firstSelectedButton;

    private void OnEnable()
    {
        FindFirstObjectByType<EventSystem>().SetSelectedGameObject(_firstSelectedButton);
    }
}
