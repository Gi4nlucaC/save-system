#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using PeraphsPizza.SaveSystem;

namespace PeraphsPizza.SaveSystem.Editor
{
    [CustomPropertyDrawer(typeof(UniqueGUID))]
    public class UniqueGUIDDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var valueProp = property.FindPropertyRelative("_value");

            const float buttonWidth = 92f;
            var fieldRect = new Rect(position.x, position.y, position.width - buttonWidth - 4f, position.height);
            var buttonRect = new Rect(fieldRect.xMax + 4f, position.y, buttonWidth, position.height);

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUI.TextField(fieldRect, label, valueProp.stringValue);
            }

            string btnLabel = string.IsNullOrEmpty(valueProp.stringValue) ? "Generate" : "Regenerate";
            if (GUI.Button(buttonRect, btnLabel))
            {
                valueProp.stringValue = System.Guid.NewGuid().ToString("N");
                property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
#endif