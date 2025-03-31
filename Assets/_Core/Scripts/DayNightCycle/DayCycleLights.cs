using UnityEngine;

public class DayCycleLights : MonoBehaviour
{
    [SerializeField] private GameObject[] _dawnLightsArray;
    [SerializeField] private GameObject[] _dayLightsArray;
    [SerializeField] private GameObject[] _afternoonLightsArray;
    [SerializeField] private GameObject[] _twillightLightsArray;
    [SerializeField] private GameObject[] _nightLightsArray;

    public void TurnOnDawnLight()
    {
        TurnOffLights();
        
        foreach(GameObject light in _dawnLightsArray)
        {
            light.SetActive(true);
        }
    }

    public void TurnOnDayLight()
    {
        TurnOffLights();

        foreach(GameObject light in _dayLightsArray)
        {
            light.SetActive(true);
        }
    }
    
    public void TurnOnAfternoonLight()
    {
        TurnOffLights();

        foreach(GameObject light in _afternoonLightsArray)
        {
            light.SetActive(true);
        }
    }
    
    public void TurnOnTwillightLight()
    {
        TurnOffLights();

        foreach(GameObject light in _twillightLightsArray)
        {
            light.SetActive(true);
        }
    }
    
    public void TurnOnNightLight()
    {
        TurnOffLights();

        foreach(GameObject light in _nightLightsArray)
        {
            light.SetActive(true);
        }
    }

    private void TurnOffLights()
    {
        foreach(GameObject light in _dawnLightsArray)
        {
            light.SetActive(false);
        }
        foreach(GameObject light in _dayLightsArray)
        {
            light.SetActive(false);
        }
        foreach(GameObject light in _afternoonLightsArray)
        {
            light.SetActive(false);
        }
        foreach(GameObject light in _twillightLightsArray)
        {
            light.SetActive(false);
        }
        foreach(GameObject light in _nightLightsArray)
        {
            light.SetActive(false);
        }
    }
}
