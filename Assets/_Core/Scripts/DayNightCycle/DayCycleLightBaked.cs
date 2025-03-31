using System.Collections.Generic;
using UnityEngine;

public class DayCycleLightBaked : MonoBehaviour
{
    [SerializeField] private Texture2D[] _lightingDawnMapDir;
    [SerializeField] private Texture2D[] _lightingDawnMapColor;
    [SerializeField] private Texture2D[] _lightingDayMapDir;
    [SerializeField] private Texture2D[] _lightingDayMapColor;

    private LightmapData[] _lightDawnMapArray;
    private LightmapData[] _lightDayMapArray;

    int dayTime = 1;

    private void Awake()
    {
        List<LightmapData> dlightmap = new List<LightmapData>();
		
		for(int i = 0; i < _lightingDawnMapDir.Length; i++)
		{
			LightmapData lmdata = new LightmapData();
			
   			lmdata.lightmapDir = _lightingDawnMapDir[i];
   			lmdata.lightmapColor = _lightingDawnMapColor[i];
			
			dlightmap.Add(lmdata);
		}
		
		_lightDawnMapArray = dlightmap.ToArray();
		
		List<LightmapData> blightmap = new List<LightmapData>();
		
		for(int i = 0; i < _lightingDayMapDir.Length; i++)
		{
			LightmapData lmdata = new LightmapData();
			
   			lmdata.lightmapDir = _lightingDayMapDir[i];
   			lmdata.lightmapColor = _lightingDayMapColor[i];
			
			blightmap.Add(lmdata);
		}
		
		_lightDayMapArray = blightmap.ToArray();
        
    }
    
    [ContextMenu("ChangeBake")]
    public void ChangeBake()
    {   
        if(dayTime == 1)
        {
            dayTime = 2;
            LightmapSettings.lightmaps = _lightDawnMapArray;
        }
        else
        {
            dayTime = 1;
            LightmapSettings.lightmaps = _lightDayMapArray;
        }

    }
}
