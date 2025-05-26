using System.Collections.Generic;
using UnityEngine;

namespace MoonlitMixes.Extensions
{
    public static class GameObjectExtensions
    {
        public static GameObject[] FindGameObjectsOfType<T>(this Object obj) where T : class
        {
            List<GameObject> objectList = new();
            foreach (var monoBehaviour in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            {
                if (monoBehaviour is T)
                {
                    objectList.Add(monoBehaviour.gameObject);
                }
            }

            if (objectList.Count > 0)
            {
                return objectList.ToArray();
            }

            return default(GameObject[]);
        }
    }
}