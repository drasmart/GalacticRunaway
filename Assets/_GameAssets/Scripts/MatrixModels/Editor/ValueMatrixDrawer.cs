using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace MatrixModels.Editor
{
    [CustomPropertyDrawer(typeof(ValueMatrix<>))]
    public class ValueMatrixDrawer : PropertyDrawer
    {
        private bool unfold = false;
        private bool flipX = false;
        private bool flipY = true;
        private bool showLabels = true;
        private bool hexGrid = false;
        private bool useSpacer = false;
        private float dx = 15;
        private const float dy = 15;
        private const float dty = 18;

        private const float dbx = 4;
        private const float dby = 3;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            var rawPosition = position;
            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            float l = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 0;

            const float wrl = 35;
            const float wcl = 52;
            const float wsl = 60;
            const float wfxl = 40;
            const float wfyl = 15;
            const float wsll = 45;
            const float wml = 11;
            const float whgl = 57;
            const float wspl = 85;

            const float dvx = 15;

            const float dfx = 12;
            const float dmx = 14;

            float vfw = (position.width - (wrl + wcl)) / 2;

            Rect rowsRect = new Rect(position.x, position.y, wrl + vfw, dty);
            Rect colsRect = new Rect(position.x + wrl + vfw, position.y, wcl + vfw, dty);

            SerializedProperty rowsProperty = property.FindPropertyRelative("rows");
            SerializedProperty colsProperty = property.FindPropertyRelative("columns");
            SerializedProperty boolProperty = property.FindPropertyRelative("matrix");

            EditorGUIUtility.labelWidth = wrl;
            EditorGUI.PropertyField(rowsRect, rowsProperty);
            EditorGUIUtility.labelWidth = wcl;
            EditorGUI.PropertyField(colsRect, colsProperty);

            Rect foldoutRect = new Rect(position.x + dfx, position.y + dty, position.width - dfx, dty);

            unfold = EditorGUI.Foldout(foldoutRect, unfold, new GUIContent("Matrix"));

            if (rowsProperty.intValue < 1) { rowsProperty.intValue = 1; }
            if (colsProperty.intValue < 1) { colsProperty.intValue = 1; }

            int r = rowsProperty.intValue;
            int c = colsProperty.intValue;

            if (unfold)
            {
                EditorGUIUtility.labelWidth = wsl;
                Rect sliderRect = new Rect(position.x + dmx, position.y + 2 * dty, position.width - dmx, dty);
                dx = EditorGUI.Slider(sliderRect, new GUIContent("Cell Witdh"), dx, 15, 200);

                float displayOptionsY = position.y + 3 * dty;

                Rect flipLblXRect      = new Rect(position.x + dmx, displayOptionsY, wfxl, dty);
                Rect xFlipRect         = new Rect(position.x + dmx + wfxl, displayOptionsY, dvx, dty);
                Rect flipLblYRect      = new Rect(position.x + dmx + wfxl + dvx, displayOptionsY, wfyl, dty);
                Rect yFlipRect         = new Rect(position.x + dmx + wfxl + wfyl + dvx, displayOptionsY, dvx, dty);
                Rect showLabelsLblRect = new Rect(position.x + dmx + wfxl + wfyl + 2 * dvx, displayOptionsY, wsll, dty);
                Rect showLabelsRect    = new Rect(position.x + dmx + wfxl + wfyl + wsll + 2 * dvx, displayOptionsY, dvx, dty);

                GUI.Label(flipLblXRect, new GUIContent("Flip X:", "Display only"));
                GUI.Label(flipLblYRect, new GUIContent("Y:", "Display only"));
                GUI.Label(showLabelsLblRect, new GUIContent("Labels:"));
                flipX = EditorGUI.Toggle(xFlipRect, flipX);
                flipY = EditorGUI.Toggle(yFlipRect, flipY);
                showLabels = EditorGUI.Toggle(showLabelsRect, showLabels);

                Rect hexGridLblRect = new Rect(position.x + dmx, displayOptionsY + dty, whgl, dty);
                Rect hexGridRect    = new Rect(position.x + dmx + whgl, displayOptionsY + dty, dvx, dty);
                Rect spacerLblRect  = new Rect(position.x + dmx + whgl + dvx, displayOptionsY + dty, wspl, dty);
                Rect spacerRect     = new Rect(position.x + dmx + whgl + wspl + dvx, displayOptionsY + dty, dvx, dty);

                GUI.Label(hexGridLblRect, new GUIContent("Hex Grid:", "Use hexagonal grid for elements"));
                hexGrid = EditorGUI.Toggle(hexGridRect, hexGrid);
                GUI.Label(spacerLblRect, new GUIContent("Group Spacer:"));
                useSpacer = EditorGUI.Toggle(spacerRect, useSpacer);
            }

            float matrixOffsetX = ((c + (showLabels ? 1 : 0) + (c / 5) * (useSpacer ? dbx : 0)) * dx < position.width - dmx) ? position.x + dmx : rawPosition.x;
            float matrixOffsetY = position.y + 5 * dty;

            if (unfold && showLabels)
            {
                if (!flipX) { matrixOffsetX += dx; }
                if (!flipY) { matrixOffsetY += dy; }
            }

            int modulationOffsetX = flipX ? c % 5 : 0;
            int modulationOffsetY = flipY ? r % 5 : 0;

            var baseOffset = new Vector2(dx, dy);
            for (int rowIndex = 0; boolProperty != null && rowIndex < r; rowIndex++)
            {
                if (boolProperty.arraySize <= rowIndex)
                {
                    boolProperty.InsertArrayElementAtIndex(rowIndex);
                }
                int vRow = rowIndex;
                if (flipY) { vRow = r - vRow - 1; };
                SerializedProperty rowProperty = boolProperty.GetArrayElementAtIndex(rowIndex);
                SerializedProperty rowDataProperty = rowProperty.FindPropertyRelative("cells");
                for (int columnIndex = 0; columnIndex < c; columnIndex++)
                {
                    if (rowDataProperty.arraySize <= columnIndex)
                    {
                        rowDataProperty.InsertArrayElementAtIndex(columnIndex);
                    }
                    if (unfold)
                    {
                        int vColumn = columnIndex;
                        if (flipX) { vColumn = c - vColumn - 1; };
                        SerializedProperty cellDataProperty = rowDataProperty.GetArrayElementAtIndex(columnIndex);
                        Vector2 offset = CellRectOffset(vRow, vColumn) * baseOffset + SpacingOffset(vRow, vColumn, modulationOffsetX, modulationOffsetY);
                        Rect cellRect = new Rect(matrixOffsetX + vColumn * dx + offset.x, matrixOffsetY + vRow * dy + offset.y, dx, dy);
                        EditorGUI.PropertyField(cellRect, cellDataProperty, GUIContent.none);
                    }
                }
                while (rowDataProperty.arraySize > c)
                {
                    rowDataProperty.DeleteArrayElementAtIndex(rowDataProperty.arraySize - 1);
                }
            }
            if (unfold && showLabels)
            {
                if (!flipX) { matrixOffsetX -= dx; }
                if (!flipY) { matrixOffsetY -= dy; }
            }
            if (unfold && showLabels)
            {
                // Horizontal Markers
                int vRow = 0;
                int mRow = 0;
                if (flipY) { vRow = r - vRow; mRow = r - mRow - 1; };
                for (int i = 0; i < c; i += 5)
                {
                    int mColumn = i;
                    int vColumn = i + 1;
                    if (flipX) { vColumn = c - vColumn; mColumn = c - mColumn - 1; };
                    Vector2 offset = CellRectOffset(mRow, mColumn) * baseOffset + SpacingOffset(mRow, mColumn, modulationOffsetX, modulationOffsetY);
                    string s = i.ToString();
                    int k = s.Length;
                    float w = k * wml;
                    Rect xLabelRect = new Rect(matrixOffsetX + vColumn * dx + offset.x + (dx - w) / 2, matrixOffsetY + vRow * dy + offset.y, w, dy);
                    GUI.Label(xLabelRect, new GUIContent(s));
                }
            }
            if (unfold && showLabels)
            {
                // Vertical Markers
                int mColumn = 0;
                int vColumn = 0;
                if (flipX) { vColumn = c - vColumn; mColumn = c - mColumn - 1; };
                for (int i = 0; i < r; i += 5)
                {
                    int vRow = i + 1;
                    int mRow = i;
                    if (flipY) { vRow = r - vRow; mRow = r - mRow - 1; };
                    Vector2 offset = CellRectOffset(mRow, mColumn) * baseOffset + SpacingOffset(mRow, mColumn, modulationOffsetX, modulationOffsetY);
                    string s = i.ToString();
                    int k = s.Length;
                    float w = k * wml;
                    Rect xLabelRect = new Rect(matrixOffsetX + vColumn * dx + offset.x + (dx - w) / 2, matrixOffsetY + vRow * dy + offset.y, w, dy);
                    GUI.Label(xLabelRect, new GUIContent(s));
                }
            }
            while (boolProperty.arraySize > r)
            {
                boolProperty.DeleteArrayElementAtIndex(boolProperty.arraySize - 1);
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            EditorGUIUtility.labelWidth = l;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty rowsProperty = property.FindPropertyRelative("rows");
            int r = rowsProperty.intValue;
            return dty * (2 + (unfold ? 2 : 0)) + dy * (unfold ? r + 1 + (showLabels ? 1 : 0) + ((r / 5) * (useSpacer ? dby : 0) / dy) : 0);
        }

        private Vector2 CellRectOffset(int row, int column)
        {
            return hexGrid ? Vector2.right * (row * 0.5f) : Vector2.zero;
        }
        private Vector2 SpacingOffset(int row, int column, int offsetX, int offsetY)
        {
            return useSpacer ? new Vector2(((column + (offsetX > 0 ? 5 - offsetX : 0)) / 5) * dbx, 
                ((row + (offsetY > 0 ? 5 - offsetY : 0)) / 5) * dby) : Vector2.zero;
        }
    }
}
