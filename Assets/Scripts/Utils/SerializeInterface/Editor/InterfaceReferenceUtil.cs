using UnityEditor;
using UnityEngine;

namespace Utils.SerializeInterface.Editor {
    public static class InterfaceReferenceUtil {
        private static GUIStyle _labelStyle;

        public static void OnGUI(Rect position, SerializedProperty property, GUIContent label, InterfaceArgs args) {
            InitializeStyleIfNeeded();
            
            /*
             * GetControlID increments each time its called, so subtracting 1 reuses the previous ID, which will ensure
             * alignment with the ID Unity assigned to the actual property field. 
             * That way the text we're about to draw is going to appear on the right control 
             */
            var controlID = GUIUtility.GetControlID(FocusType.Passive) - 1;
            var isHovering = position.Contains(Event.current.mousePosition);
            var displayString = property.objectReferenceValue == null || isHovering ? $"({args.InterfaceType.Name})" : "*";
            DrawInterfaceNameLabel(position, displayString, controlID);
        }

        private static void DrawInterfaceNameLabel(Rect position, string displayString, int controlID) {
            if (Event.current.type == EventType.Repaint) {
                const int additionalLeftWidth = 3;
                const int verticalIndent = 1;
            
                var content = EditorGUIUtility.TrTextContent(displayString);
                var size = _labelStyle.CalcSize(content);
                var labelPos = position;
                
                labelPos.width = size.x + additionalLeftWidth;
                /*
                 * Aligns the custom label to the right side of the ObjectField.
                 * It starts at the field's right edge (labelPos.x + position.width) and moves left by the label's width
                 * and an additional 18 pixels to account for the object picker icon, preventing any overlap.
                 */
                labelPos.x += position.width - labelPos.width - 18;
                labelPos.height -= verticalIndent * 2;
                labelPos.y += verticalIndent;
                _labelStyle.Draw(labelPos, EditorGUIUtility.TrTextContent(displayString), controlID, DragAndDrop.activeControlID == controlID, false);
            }
        }

        private static void InitializeStyleIfNeeded() {
            if (_labelStyle != null) return;
        
            var style = new GUIStyle(EditorStyles.label) {
                font = EditorStyles.objectField.font,
                fontSize = EditorStyles.objectField.fontSize,
                fontStyle = EditorStyles.objectField.fontStyle,
                alignment = TextAnchor.MiddleRight,
                padding = new RectOffset(0, 2, 0, 0)
            };
            _labelStyle = style;
        }
    }
}