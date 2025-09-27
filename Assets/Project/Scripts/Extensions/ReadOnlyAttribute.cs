using UnityEditor;
using UnityEngine;

namespace Project.Scripts.Extensions
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
    
    [CustomPropertyDrawer (typeof (ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndDisabledGroup();
        }
    }
}