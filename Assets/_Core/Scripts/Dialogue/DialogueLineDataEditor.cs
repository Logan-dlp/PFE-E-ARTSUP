using UnityEditor;

namespace MoonlitMixes.Datas
{
    [CustomEditor(typeof(DialogueLineData))]
    public class DialogueLineDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var dialogueLineData = (DialogueLineData)target;

            DrawDefaultInspector();

            if (dialogueLineData.IsTrembleEffect)
            {
                EditorGUILayout.Space();
                dialogueLineData.TrembleIntensityX = EditorGUILayout.Slider("Tremble Intensity X", dialogueLineData.TrembleIntensityX, 0f, 10f);

                dialogueLineData.TrembleIntensityY = EditorGUILayout.Slider("Tremble Intensity Y", dialogueLineData.TrembleIntensityY, 0f, 10f);
            }
        }
    }
}