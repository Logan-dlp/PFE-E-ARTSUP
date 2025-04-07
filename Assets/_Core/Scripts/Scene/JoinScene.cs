using MoonlitMixes.DayNightCycle;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinScene : MonoBehaviour
{
    [SerializeField] private string _sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(_sceneToLoad);
            GetComponent<UpdateDayCycle>().UpdateDayPhase();
        }
    }
}