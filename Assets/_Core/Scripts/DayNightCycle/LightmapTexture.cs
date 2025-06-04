using UnityEngine;

namespace MoonlitMixes.DayNightCycle
{
    [System.Serializable]
    public class LightmapTexture
    {
        [SerializeField] internal Texture2D[] _lightingMapDir;
        [SerializeField] internal Texture2D[] _lightingMapColor;

        internal LightmapData[] _lightMapArray;
    }
}