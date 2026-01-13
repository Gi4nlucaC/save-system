using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SlotsManager))]
public class SlotsManagerEditor : Editor
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
            EditorGUILayout.LabelField("Cloud Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_username, new GUIContent("Username"));
            EditorGUILayout.PropertyField(_password, new GUIContent("Password"));
            EditorGUILayout.PropertyField(_clusterName, new GUIContent("Cluster Name"));
        }
        else
        {
            EditorGUILayout.PropertyField(_serializationType);
        }

        EditorGUILayout.PropertyField(_folderName);

        serializedObject.ApplyModifiedProperties();
    }

}
