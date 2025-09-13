using UnityEngine;
using UnityEditor;

namespace PizzaCompany.SaveSystem.Editor
{
    [CustomEditor(typeof(SlotsManager))]
    public class SlotsManagerEditor : UnityEditor.Editor
    {
        SerializedProperty _userId;
        SerializedProperty _saveInCloud;
        SerializedProperty _serializationType;
        SerializedProperty _folderName;
        SerializedProperty _cloudCredentials;
        SerializedProperty _databaseName;

        void OnEnable()
        {
            _userId = serializedObject.FindProperty("_userId");
            _saveInCloud = serializedObject.FindProperty("_saveInCloud");
            _serializationType = serializedObject.FindProperty("_serializationType");
            _folderName = serializedObject.FindProperty("_folderName");
            _cloudCredentials = serializedObject.FindProperty("_cloudCredentials");
            _databaseName = serializedObject.FindProperty("_databaseName");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_userId);
            EditorGUILayout.PropertyField(_saveInCloud);

            if (_saveInCloud.boolValue)
            {
                EditorGUILayout.LabelField("Cloud Settings", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(_cloudCredentials, new GUIContent("Credentials Asset"));
                EditorGUILayout.PropertyField(_databaseName, new GUIContent("Database Name"));

                if (_cloudCredentials.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox(
                        "Assign a CloudSaveCredentials asset (store it in an ignored folder like Assets/Secrets). " +
                        "Alternatively set environment variables SAVE_SYSTEM_DB_USER / PASS / CLUSTER for builds.",
                        MessageType.Info);
                }
                else
                {
                    var cred = (CloudSaveCredentials)_cloudCredentials.objectReferenceValue;
                    if (!cred.HasValidCredentials)
                        EditorGUILayout.HelpBox("Credentials asset has empty fields.", MessageType.Warning);
                }
            }
            else
            {
                EditorGUILayout.PropertyField(_serializationType);
            }

            EditorGUILayout.PropertyField(_folderName);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
