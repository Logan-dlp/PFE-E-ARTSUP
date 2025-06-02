using UnityEngine;

namespace MoonlitMixes.Datas
{
    [CreateAssetMenu(fileName = "DayNightCycleInfo", menuName = "Scriptable Objects/DayNightCycleInfo")]
    public class DayNightCycleInfo : ScriptableObject
    {
        [SerializeField] private Material _skyboxDawn;
        [SerializeField] private Material _skyboxDay;
        [SerializeField] private Material _skyBoxAfternoon;
        [SerializeField] private Material _skyBoxTwillight;
        [SerializeField] private Material _skyboxNight;        
        [SerializeField] private int _actualTimePhase;
        [SerializeField] private int _actualDay;

        public int ActualDay
        {
            get => _actualDay;
            set => _actualDay = Mathf.Clamp(value, 0, 3);
        }
        
        public Material SkyboxDawn
        {
            get => _skyboxDawn;
            set => _skyboxDawn = value;
        }
        public Material SkyboxDay
        {
            get => _skyboxDay;
            set => _skyboxDay = value;
        }
        public Material SkyBoxAfternoon
        {
            get => _skyBoxAfternoon;
            set => _skyBoxAfternoon = value;
        }
        public Material SkyBoxTwillight
        {
            get => _skyBoxTwillight;
            set => _skyBoxTwillight = value;
        }
        public Material SkyboxNight
        {
            get => _skyboxNight;
            set => _skyboxNight = value;
        }
        public int ActualTimePhase
        {
            get => _actualTimePhase;
            set => _actualTimePhase = Mathf.Clamp(value, 0, 3);
        }
    }
}
