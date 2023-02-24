using UnityEditor;
using UnityEngine;


namespace LobstersUnited.UnityXState.Editor {

    [CustomPropertyDrawer(typeof(StateMachineEditorHelper))]
    internal class StateMachineEditor : PropertyDrawer {

        static readonly string CURRENT_STATES_LABEL = "Current States";

        float totalHeight;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var stateMachine = EditorUtils.GetParentObjectOfProperty(property);
            Debug.Log(stateMachine);

            var height = DrawCurrentStatesInfo(position);
            
            totalHeight = height;
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return totalHeight;
        }
        
        float DrawCurrentStatesInfo(Rect pos) {
            var startY = pos.y;
            
            
            pos.y += DrawCurrentStatesHeader(pos);
            
            
            
            // var id = GUIUtility.GetControlID(FocusType.Keyboard, pos);
            // var startY = pos.y;
            //
            // var label = new GUIContent(CURRENT_STATES_LABEL);
            // var labelPos = pos;
            // var labelStyle = new GUIStyle(EditorStyles.label);
            //
            // var fieldPos = EditorGUI.PrefixLabel(labelPos, id, new GUIContent(label), labelStyle);
            // labelPos.width = EditorGUIUtility.labelWidth;
            //
            // // var fieldStyle = new GUIStyle(EditorStyles.objectField);
            // // if (Event.current.type == EventType.Repaint) {
            // //     fieldStyle.Draw(fieldPos, content, id, DrawerUtils.HasDnD(id), isHovered);
            // // }
            //
            // pos.y += EditorGUIUtility.singleLineHeight;
            
            return pos.y - startY;
        }

        static float DrawCurrentStatesHeader(Rect pos) {
            var startY = pos.y;
            
            EditorGUI.LabelField(pos, CURRENT_STATES_LABEL);
            pos.y += EditorGUIUtility.singleLineHeight;
            pos.y += EditorGUIUtility.standardVerticalSpacing;
            
            return pos.y - startY;
        }
        
    }
    
}
