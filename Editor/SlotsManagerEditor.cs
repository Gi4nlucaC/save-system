using UnityEngine;
using UnityEditor;

namespace SaveSystem.Editor
{
    /// <summary>
    /// Custom editor for SlotsManager to provide better inspector experience
    /// </summary>
    [CustomEditor(typeof(SlotsManager))]
    public class SlotsManagerEditor : UnityEditor.Editor
    {
        SerializedProperty _userId;
        SerializedProperty _saveInCloud;
        SerializedProperty _serializationType;
        SerializedProperty _folderName;

        SerializedProperty _username;
        SerializedProperty _password;
        SerializedProperty _clusterName;

        private void OnEnable()
        {
            _userId = serializedObject.FindProperty("_userId");
            _saveInCloud = serializedObject.FindProperty("_saveInCloud");
            _serializationType = serializedObject.FindProperty("_serializationType");
            _folderName = serializedObject.FindProperty("_folderName");

            _username = serializedObject.FindProperty("_username");
            _password = serializedObject.FindProperty("_password");
            _clusterName = serializedObject.FindProperty("_clusterName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_userId);
            EditorGUILayout.PropertyField(_saveInCloud);

            if (_saveInCloud.boolValue)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Cloud Settings", EditorStyles.boldLabel);
                
                EditorGUILayout.HelpBox(
                    "Warning: Cloud credentials are currently exposed in the code. " +
                    "This needs to be secured before production deployment.", 
                    MessageType.Warning);
                    
                EditorGUILayout.PropertyField(_username, new GUIContent("Username"));
                EditorGUILayout.PropertyField(_password, new GUIContent("Password"));
                EditorGUILayout.PropertyField(_clusterName, new GUIContent("Cluster Name"));
            }
            else
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Local Save Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_serializationType);
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(_folderName);

            serializedObject.ApplyModifiedProperties();
        }
    }
}