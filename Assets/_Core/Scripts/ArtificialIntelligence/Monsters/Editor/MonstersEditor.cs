#if UNITY_EDITOR

using System;
using UnityEditor;

namespace MoonlitMixes.AI.Editor
{
    [CustomEditor(typeof(Monsters))]
    public class MonstersEditor : UnityEditor.Editor
    {
        private const float MIN_DETECTION = 0;
        private const float MAX_DETECTION = 10;
        
        private SerializedProperty _comportementProperty;
        private SerializedProperty _attackRadiusProperty;
        private SerializedProperty _detectionStopProperty;
        
        private void OnEnable()
        {
            Monsters monsters = (Monsters)target;
            serializedObject.Update();
            
            _comportementProperty = serializedObject.FindProperty("_comportement");
            _attackRadiusProperty = serializedObject.FindProperty("_attackRadius");
            _detectionStopProperty = serializedObject.FindProperty("_detectionStop");
        }

        public override void OnInspectorGUI()
        {
            _comportementProperty.enumValueIndex = EditorGUILayout.Popup("Comportement", _comportementProperty.enumValueIndex, _comportementProperty.enumDisplayNames);
            
            if (_comportementProperty.enumValueIndex == (int)MonstersComportement.Aggressive)
            {
                _attackRadiusProperty.floatValue = EditorGUILayout.Slider("Atack Radius", _attackRadiusProperty.floatValue, MIN_DETECTION, MAX_DETECTION);
                _detectionStopProperty.floatValue = EditorGUILayout.Slider("Detection Stop", _detectionStopProperty.floatValue, _attackRadiusProperty.floatValue, MAX_DETECTION + _attackRadiusProperty.floatValue);
            }
            else
            {
                _detectionStopProperty.floatValue = EditorGUILayout.Slider("Detection Stop", _detectionStopProperty.floatValue, MIN_DETECTION, MAX_DETECTION);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif