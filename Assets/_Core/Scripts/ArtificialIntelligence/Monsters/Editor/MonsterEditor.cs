#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace MoonlitMixes.AI.Editor
{
    [CustomEditor(typeof(Monster))]
    public class MonsterEditor : UnityEditor.Editor
    {
        private const float MIN_DETECTION = 0;
        private const float MAX_DETECTION = 10;

        private SerializedProperty _comportementProperty;
        private SerializedProperty _speedAttackProperty;
        private SerializedProperty _stopDistanceToAttackProperty;
        private SerializedProperty _attackRadiusProperty;
        private SerializedProperty _detectionStopProperty;

        private SerializedProperty _attackDamageProperty;
        private SerializedProperty _attackForceProperty;
        private SerializedProperty _attackDurationProperty;

        private SerializedProperty _soundAttackBat;
        private SerializedProperty _soundMoveBat;
        private SerializedProperty _soundAttackGolem;
        private SerializedProperty _soundMoveGolem;
        private SerializedProperty _soundAttackSlime;
        private SerializedProperty _soundMoveSlime;
        private SerializedProperty _soundAttackWillowraith;
        private SerializedProperty _soundMoveWillowraith;
        private SerializedProperty _soundDeathSmallEnemy;
        private SerializedProperty _soundDeathBigEnemy;

        private bool _isViewDetailsOpened = false;

        private void OnEnable()
        {
            Monster monster = (Monster)target;
            serializedObject.Update();

            _comportementProperty = serializedObject.FindProperty("_comportement");
            _speedAttackProperty = serializedObject.FindProperty("_speedAttack");
            _stopDistanceToAttackProperty = serializedObject.FindProperty("_stopDistanceToAttack");
            _attackRadiusProperty = serializedObject.FindProperty("_attackRadius");
            _detectionStopProperty = serializedObject.FindProperty("_detectionStop");

            _attackDamageProperty = serializedObject.FindProperty("_attackDamage");
            _attackForceProperty = serializedObject.FindProperty("_attackForce");
            _attackDurationProperty = serializedObject.FindProperty("_attackDuration");

            _soundAttackBat = serializedObject.FindProperty("_soundAttackBat");
            _soundMoveBat = serializedObject.FindProperty("_soundMoveBat");
            _soundAttackGolem = serializedObject.FindProperty("_soundAttackGolem");
            _soundMoveGolem = serializedObject.FindProperty("_soundMoveGolem");
            _soundAttackSlime = serializedObject.FindProperty("_soundAttackSlime");
            _soundMoveSlime = serializedObject.FindProperty("_soundMoveSlime");
            _soundAttackWillowraith = serializedObject.FindProperty("_soundAttackWillowraith");
            _soundMoveWillowraith = serializedObject.FindProperty("_soundMoveWillowraith");
            _soundDeathSmallEnemy = serializedObject.FindProperty("_soundDeathSmallEnemy");
            _soundDeathBigEnemy = serializedObject.FindProperty("_soundDeathBigEnemy");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Comportement", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            _comportementProperty.enumValueIndex = EditorGUILayout.Popup("Comportement", _comportementProperty.enumValueIndex, _comportementProperty.enumDisplayNames);
            
            _speedAttackProperty.floatValue = EditorGUILayout.FloatField("Speed Movement Attack", _speedAttackProperty.floatValue);
            _stopDistanceToAttackProperty.floatValue = EditorGUILayout.FloatField("Stop Distance To Attack", _stopDistanceToAttackProperty.floatValue);

            if (_comportementProperty.enumValueIndex == (int)MonsterComportement.Aggressive)
            {
                _attackRadiusProperty.floatValue = EditorGUILayout.Slider("Attack Radius", _attackRadiusProperty.floatValue, MIN_DETECTION, MAX_DETECTION);
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

            EditorGUILayout.Space(12);
            EditorGUILayout.LabelField("FMOD Audio Events", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Bat", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(_soundAttackBat, new GUIContent("Attack"));
            EditorGUILayout.PropertyField(_soundMoveBat, new GUIContent("Move"));

            EditorGUILayout.LabelField("Golem", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(_soundAttackGolem, new GUIContent("Attack"));
            EditorGUILayout.PropertyField(_soundMoveGolem, new GUIContent("Move"));

            EditorGUILayout.LabelField("Slime", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(_soundAttackSlime, new GUIContent("Attack"));
            EditorGUILayout.PropertyField(_soundMoveSlime, new GUIContent("Move"));

            EditorGUILayout.LabelField("Willowraith", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(_soundAttackWillowraith, new GUIContent("Attack"));
            EditorGUILayout.PropertyField(_soundMoveWillowraith, new GUIContent("Move"));

            EditorGUILayout.LabelField("Death", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(_soundDeathSmallEnemy, new GUIContent("Small Enemy Death"));
            EditorGUILayout.PropertyField(_soundDeathBigEnemy, new GUIContent("Big Enemy Death"));
        }
    }
}
#endif