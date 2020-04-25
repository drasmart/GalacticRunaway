using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexGrid
{
    [CustomPropertyDrawer(typeof(HexCoords3))]
    public class HexCoords3Drawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            //// Don't make child fields be indented
            //var indent = EditorGUI.indentLevel;
            //EditorGUI.indentLevel = 0;
            //var lw = EditorGUIUtility.labelWidth;

            var u = property.FindPropertyRelative("u");
            var v = property.FindPropertyRelative("v");
            var y = property.FindPropertyRelative("y");

            var t = new float[] { u.floatValue, v.floatValue, y.floatValue };
            EditorGUI.MultiFloatField(position, new GUIContent[] { new GUIContent("U"), new GUIContent("V"), new GUIContent("Y") }, t);

            u.floatValue = t[0];
            v.floatValue = t[1];
            y.floatValue = t[2];

            //// Set indent back to what it was
            //EditorGUI.indentLevel = indent;
            //EditorGUIUtility.labelWidth = lw;

            EditorGUI.EndProperty();
        }
    }
}
