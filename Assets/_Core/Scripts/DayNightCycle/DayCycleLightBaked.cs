using System.Collections.Generic;
using MoonlitMixes.Datas;
using UnityEngine;

public class DayCycleLightBaked : MonoBehaviour
{
    [SerializeField] private LightmapTexture[] _lightMapTextureArray;
    [SerializeField] private int _timePhaseToChangeBake;
    [SerializeField] private DayNightCycleInfo _dayNightCycleInfo;

    private int _dayTime = 0;

    public int DayTime
    {
        get => _dayTime;
        set => _dayTime = value;
    }

    private void Awake()
    {
        for (int i = 0; i < _lightMapTextureArray.Length; i++)
        {
            List<LightmapData> lightmap = new List<LightmapData>();

            for (int j = 0; j < _lightMapTextureArray[i]._lightingMapDir.Length; j++)
            {
                LightmapData lmdata = new LightmapData();

                lmdata.lightmapDir = _lightMapTextureArray[i]._lightingMapDir[j];
                lmdata.lightmapColor = _lightMapTextureArray[i]._lightingMapColor[j];

                lightmap.Add(lmdata);
            }

            _lightMapTextureArray[i]._lightMapArray = lightmap.ToArray();
        }

        ChangeBake();
    }

    public void ChangeBake()
    {
        if (_dayNightCycleInfo.ActualTimePhase != _timePhaseToChangeBake)
        {
            LightmapSettings.lightmaps = _lightMapTextureArray[1]._lightMapArray;
        }
        else
        {
            LightmapSettings.lightmaps = _lightMapTextureArray[0]._lightMapArray;
        }
    }

    [System.Serializable]
    private class LightmapTexture
    {
        [SerializeField] internal Texture2D[] _lightingMapDir;
        [SerializeField] internal Texture2D[] _lightingMapColor;

        internal LightmapData[] _lightMapArray;
    }
}
