using UnityEngine;

public class DayCycleLights : MonoBehaviour
{
    [SerializeField] private GameObject[] _dawnLightsArray;
    [SerializeField] private GameObject[] _dayLightsArray;
    [SerializeField] private GameObject[] _afternoonLightsArray;
    [SerializeField] private GameObject[] _twillightLightsArray;
    [SerializeField] private GameObject[] _nightLightsArray;

    public void TurnOnDawnLight() => TrunOnLight(0);

    public void TurnOnDayLight() => TrunOnLight(1);
    
    public void TurnOnAfternoonLight() => TrunOnLight(2);
        
    public void TurnOnTwillightLight() => TrunOnLight(3);

    public void TurnOnNightLight() => TrunOnLight(4);
    
    private void TrunOnLight(int dayTime)
    {
        TurnOffLights();

        switch(dayTime)
        {
            case 0:
                foreach(GameObject light in _dawnLightsArray)
                {
                    light.SetActive(true);
                }
                break;
            case 1:
                foreach(GameObject light in _dayLightsArray)
                {
                    light.SetActive(true);
                }
                break;
            case 2:
                foreach(GameObject light in _afternoonLightsArray)
                {
                    light.SetActive(true);
                }
                break;
            case 3:
                foreach(GameObject light in _twillightLightsArray)
                {
                    light.SetActive(true);
                }
                break;
            default:
                foreach(GameObject light in _nightLightsArray)
                {
                    light.SetActive(true);
                }
                break;
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
