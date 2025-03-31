#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace MoonlitMixes.Camera.Event
{
    [CustomEditor(typeof(TriggerCameraEvent))]
    public class TriggerCameraEventEditor : Editor
    {
        private SerializedProperty _cameraEvent;
        private SerializedProperty _cameraToTransition;
        private SerializedProperty _zoomTarget;

        private void OnEnable()
        {
            _cameraEvent = serializedObject.FindProperty("_cameraEvent");
            _cameraToTransition = serializedObject.FindProperty("_cameraToTransition");
            _zoomTarget = serializedObject.FindProperty("_zoomTarget");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_cameraEvent);

            switch (_cameraEvent.enumValueIndex)
            {
                case 1 :
                    EditorGUILayout.PropertyField(_zoomTarget);
                    break;
                case 2 :
                    EditorGUILayout.PropertyField(_cameraToTransition);
                    break;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif