using UnityEngine;

namespace MoonlitMixes.Extensions
{
    public static class TransformExtension
    {
        public static void DestroyAllChild(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}