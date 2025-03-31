using System.Collections.Generic;
using UnityEngine;

public class DayCycleLightBaked : MonoBehaviour
{
    [SerializeField] private LightmapTexture[] _lightMapTextureArray;

    private int _dayTime = 0;

    public int DayTime
    {
        get => _dayTime;
        set => _dayTime = value;
    }

    private void Awake()
    {
        for(int i = 0; i < _lightMapTextureArray.Length; i++)
        {
            List<LightmapData> lightmap = new List<LightmapData>();
		    
            for(int j = 0; j < _lightMapTextureArray[i]._lightingMapDir.Length; j++)
		    {
		    	LightmapData lmdata = new LightmapData();
    
   		    	lmdata.lightmapDir = _lightMapTextureArray[i]._lightingMapDir[j];
   		    	lmdata.lightmapColor = _lightMapTextureArray[i]._lightingMapColor[j];
    
		    	lightmap.Add(lmdata);
		    }
		    
            _lightMapTextureArray[i]._lightMapArray = lightmap.ToArray();
        }    
    }
    
    [ContextMenu("ChangeBake")]
    public void ChangeBake(/*int dayTime*/)
    {   
        LightmapSettings.lightmaps = _lightMapTextureArray[_dayTime]._lightMapArray;
        
        if(_dayTime == 1) 
        {
            _dayTime = 0; //C'est juste là pour les tests, faudra le retirer plus tard
            Debug.Log("");
        }
        else _dayTime++;
    }

    [System.Serializable]
    public class LightmapTexture
    {
        [SerializeField] internal Texture2D[] _lightingMapDir;
        [SerializeField] internal Texture2D[] _lightingMapColor;

        internal LightmapData[] _lightMapArray;
    }
}
