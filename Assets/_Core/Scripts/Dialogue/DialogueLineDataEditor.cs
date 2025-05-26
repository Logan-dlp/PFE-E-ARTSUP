#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MoonlitMixes.Datas
{
    [CustomEditor(typeof(DialogueLineData))]
    public class DialogueLineDataEditor : Editor
    {
        SerializedProperty _effectProp;
        SerializedProperty _trembleXProp;
        SerializedProperty _trembleYProp;
        SerializedProperty _fadeInDurationProp;
        SerializedProperty _fadeOutDurationProp;

        void OnEnable()
        {
            _effectProp = serializedObject.FindProperty("_effect");
            _trembleXProp = serializedObject.FindProperty("_trembleIntensityX");
            _trembleYProp = serializedObject.FindProperty("_trembleIntensityY");
            _fadeInDurationProp = serializedObject.FindProperty("_fadeInDuration");
            _fadeOutDurationProp = serializedObject.FindProperty("_fadeOutDuration");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_text"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_speakerIndex"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_speakerSprite"));
            EditorGUILayout.PropertyField(_effectProp);

            var effect = (SpeakerEffectType)_effectProp.enumValueIndex;

            if (effect == SpeakerEffectType.Tremble)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Slider(_trembleXProp, 0f, 10f, new GUIContent("Tremble Intensity X"));
                EditorGUILayout.Slider(_trembleYProp, 0f, 10f, new GUIContent("Tremble Intensity Y"));
            }
            else if (effect == SpeakerEffectType.FadeIn)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Slider(_fadeInDurationProp, 0.1f, 5f, new GUIContent("Fade In Duration"));
            }
            else if (effect == SpeakerEffectType.FadeOut)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Slider(_fadeOutDurationProp, 0.1f, 5f, new GUIContent("Fade Out Duration"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif