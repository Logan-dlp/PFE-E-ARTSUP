using UnityEngine;
using System.Collections;

namespace MoonlitMixes.UI
{
    public class FpsCounter : MonoBehaviour
    {
        private float _count;
        private GUIStyle _guiStyle = new GUIStyle();
        
        private IEnumerator Start()
        {
            GUI.depth = 2;

            while (true)
            {
                _count = 1f / Time.unscaledDeltaTime;
                yield return new WaitForSeconds(0.1f);
                }
        }

        private void OnGUI()
        {
            _guiStyle.fontSize = 100;
            GUI.Label(new Rect(5, 40, 100, 25), "FPS: " + Mathf.Round(_count), _guiStyle);
        }
    }
}
