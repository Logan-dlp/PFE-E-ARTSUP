#if UNITY_EDITOR

using UnityEditor;

namespace MoonlitMixes.AI.Editor
{
    [CustomEditor(typeof(Monster))]
    public class MonsterEditor : UnityEditor.Editor
    {
        private const float MIN_DETECTION = 0;
        private const float MAX_DETECTION = 10;
        
        private SerializedProperty _comportementProperty;
        private SerializedProperty _stopDistanceToAttackProperty;
        private SerializedProperty _attackRadiusProperty;
        private SerializedProperty _detectionStopProperty;

        private SerializedProperty _attackDamageProperty;
        private SerializedProperty _attackForceProperty;
        private SerializedProperty _attackDurationProperty;
        
        private SerializedProperty _healthProperty;
        
        private bool _isViewDetailsOpened = false;
        
        private void OnEnable()
        {
            Monster monster = (Monster)target;
            serializedObject.Update();
            
            _comportementProperty = serializedObject.FindProperty("_comportement");
            _stopDistanceToAttackProperty = serializedObject.FindProperty("_stopDistanceToAttack");
            _attackRadiusProperty = serializedObject.FindProperty("_attackRadius");
            _detectionStopProperty = serializedObject.FindProperty("_detectionStop");

            _attackDamageProperty = serializedObject.FindProperty("_attackDamage");
            _attackForceProperty = serializedObject.FindProperty("_attackForce");
            _attackDurationProperty = serializedObject.FindProperty("_attackDuration");
            
            _healthProperty = serializedObject.FindProperty("_health");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Comportement", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            _comportementProperty.enumValueIndex = EditorGUILayout.Popup("Comportement", _comportementProperty.enumValueIndex, _comportementProperty.enumDisplayNames);

            _stopDistanceToAttackProperty.floatValue = EditorGUILayout.FloatField("Stop Distance To Attack", _stopDistanceToAttackProperty.floatValue);
            
            if (_comportementProperty.enumValueIndex == (int)MonsterComportement.Aggressive)
            {
                _attackRadiusProperty.floatValue = EditorGUILayout.Slider("Atack Radius", _attackRadiusProperty.floatValue, MIN_DETECTION, MAX_DETECTION);
                _detectionStopProperty.floatValue = EditorGUILayout.Slider("Detection Stop", _detectionStopProperty.floatValue, _attackRadiusProperty.floatValue, MAX_DETECTION + _attackRadiusProperty.floatValue);
            }
            else
            {
                _detectionStopProperty.floatValue = EditorGUILayout.Slider("Detection Stop", _detectionStopProperty.floatValue, MIN_DETECTION, MAX_DETECTION);
            }
            
            EditorGUILayout.Space(12);
            EditorGUILayout.LabelField("Attack", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _attackDamageProperty.intValue = EditorGUILayout.IntField("Attack Damage", _attackDamageProperty.intValue);
            
            _isViewDetailsOpened = EditorGUILayout.Toggle("View details", _isViewDetailsOpened);
            if (_isViewDetailsOpened)
            {
                _attackForceProperty.floatValue = EditorGUILayout.FloatField("Attack Force", _attackForceProperty.floatValue);
                _attackDurationProperty.floatValue = EditorGUILayout.FloatField("Attack Duration", _attackDurationProperty.floatValue);
            }
            else
            {
                _attackForceProperty.floatValue = 2;
                _attackDurationProperty.floatValue = .45f;
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif