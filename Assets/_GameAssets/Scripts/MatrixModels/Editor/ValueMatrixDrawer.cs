using UnityEngine;
using UnityEditor;

namespace MatrixModels.Editor
{
    [CustomPropertyDrawer(typeof(ValueMatrix<>))]
    [CustomPropertyDrawer(typeof(MatrixDisplayAttribute))]
    public class ValueMatrixDrawer : PropertyDrawer
    {
        private bool flipX;
        private bool flipY = true;
        private bool showLabels = true;
        private bool hexGrid;
        private bool useSpacer;
        private float dx = 15;
        
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Cell line height
        /// </summary>
        private const float dy = 15;
        /// <summary>
        /// Text line height
        /// </summary>
        private const float dty = 18;
        /// <summary>
        /// X group spacer width
        /// </summary>
        private const float dbx = 4;
        /// <summary>
        /// Y group spacer height
        /// </summary>
        private const float dby = 3;
        // ReSharper restore InconsistentNaming

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool hasLockedSettings = false;
            if (attribute is MatrixDisplayAttribute displayAttribute)
            {
                flipX = displayAttribute.InvertX;
                flipY = displayAttribute.YUp;
                hexGrid = displayAttribute.Hexagonal;
                hasLockedSettings = true;
            }
            
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
            SerializedProperty matrixProperty = property.FindPropertyRelative("matrix");

            EditorGUIUtility.labelWidth = wrl;
            EditorGUI.PropertyField(rowsRect, rowsProperty);
            EditorGUIUtility.labelWidth = wcl;
            EditorGUI.PropertyField(colsRect, colsProperty);

            Rect foldoutRect = new Rect(position.x + dfx, position.y + dty, position.width - dfx, dty);

            bool unfold = property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, new GUIContent("Matrix"));

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
                GUI.Label(showLabelsLblRect, new GUIContent("Labels:", "Display only"));

                Rect hexGridLblRect = new Rect(position.x + dmx, displayOptionsY + dty, whgl, dty);
                Rect hexGridRect    = new Rect(position.x + dmx + whgl, displayOptionsY + dty, dvx, dty);
                Rect spacerLblRect  = new Rect(position.x + dmx + whgl + dvx, displayOptionsY + dty, wspl, dty);
                Rect spacerRect     = new Rect(position.x + dmx + whgl + wspl + dvx, displayOptionsY + dty, dvx, dty);

                GUI.Label(hexGridLblRect, new GUIContent("Hex Grid:", "Use hexagonal grid for elements"));
                GUI.Label(spacerLblRect, new GUIContent("Group Spacer:"));

                using (var _ = new EditorGUI.DisabledScope(hasLockedSettings)){
                    flipX = EditorGUI.Toggle(xFlipRect, flipX);
                    flipY = EditorGUI.Toggle(yFlipRect, flipY);
                    hexGrid = EditorGUI.Toggle(hexGridRect, hexGrid);
                }
                showLabels = EditorGUI.Toggle(showLabelsRect, showLabels);
                useSpacer = EditorGUI.Toggle(spacerRect, useSpacer);
            }

            int xGroups = c / 5;
            float matrixOffsetX = ((c + (showLabels ? 1 : 0) + xGroups * (useSpacer ? dbx : 0)) * dx < position.width - dmx) ? position.x + dmx : rawPosition.x;
            float matrixOffsetY = position.y + 5 * dty;

            if (unfold && showLabels)
            {
                if (!flipX) { matrixOffsetX += dx; }
                if (!flipY) { matrixOffsetY += dy; }
            }

            int modulationOffsetX = flipX ? c % 5 : 0;
            int modulationOffsetY = flipY ? r % 5 : 0;

            var baseOffset = new Vector2(dx, dy);
            for (int rowIndex = 0; matrixProperty != null && rowIndex < r; rowIndex++)
            {
                if (matrixProperty.arraySize <= rowIndex)
                {
                    matrixProperty.InsertArrayElementAtIndex(rowIndex);
                }
                int vRow = rowIndex;
                if (flipY) { vRow = r - vRow - 1; }
                SerializedProperty rowProperty = matrixProperty.GetArrayElementAtIndex(rowIndex);
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
                        if (flipX) { vColumn = c - vColumn - 1; }
                        SerializedProperty cellDataProperty = rowDataProperty.GetArrayElementAtIndex(columnIndex);
                        Vector2 offset = CellRectOffset(vRow) * baseOffset + SpacingOffset(vRow, vColumn, modulationOffsetX, modulationOffsetY);
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
                int vRow = flipY ? r : 0;
                int mRow = flipY ? r - 1 : 0;
                for (int i = 0; i < c; i += 5)
                {
                    int mColumn = i;
                    int vColumn = i + 1;
                    if (flipX) { vColumn = c - vColumn; mColumn = c - mColumn - 1; }
                    Vector2 offset = CellRectOffset(mRow) * baseOffset + SpacingOffset(mRow, mColumn, modulationOffsetX, modulationOffsetY);
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
                int mColumn = flipX ? c : 0;
                int vColumn = flipX ? c - 1 : 0;
                for (int i = 0; i < r; i += 5)
                {
                    int vRow = i + 1;
                    int mRow = i;
                    if (flipY) { vRow = r - vRow; mRow = r - mRow - 1; }
                    Vector2 offset = CellRectOffset(mRow) * baseOffset + SpacingOffset(mRow, mColumn, modulationOffsetX, modulationOffsetY);
                    string s = i.ToString();
                    int k = s.Length;
                    float w = k * wml;
                    Rect xLabelRect = new Rect(matrixOffsetX + vColumn * dx + offset.x + (dx - w) / 2, matrixOffsetY + vRow * dy + offset.y, w, dy);
                    GUI.Label(xLabelRect, new GUIContent(s));
                }
            }

            if (matrixProperty != null)
            {
                while (matrixProperty.arraySize > r)
                {
                    matrixProperty.DeleteArrayElementAtIndex(matrixProperty.arraySize - 1);
                }
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            EditorGUIUtility.labelWidth = l;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float result = 2 * dty;
            if (property.isExpanded)
            {
                result += 2 * dty;
                SerializedProperty rowsProperty = property.FindPropertyRelative("rows");
                int r = rowsProperty.intValue;
                float visualRows = r + 1;
                if (showLabels)
                {
                    visualRows += 1;
                }
                if (useSpacer)
                {
                    int spacersCount = r / 5;
                    visualRows += spacersCount * dby / dy;
                }
                result += dy * visualRows;
            }
            return result;
        }

        private Vector2 CellRectOffset(int row)
        {
            return hexGrid ? Vector2.right * (row * 0.5f) : Vector2.zero;
        }
        private Vector2 SpacingOffset(int row, int column, int offsetX, int offsetY)
        {
            if (!useSpacer)
            {
                return Vector2.zero;
            }

            var coords = new Vector2Int(column, row);
            if (offsetX > 0)
            {
                coords.x += (5 - offsetX) / 5;
            }
            if (offsetY > 0)
            {
                coords.y += (5 - offsetY) / 5;
            }
            return new Vector2(coords.x * dbx, coords.y * dby);
        }
    }
}
