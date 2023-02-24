using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace LobstersUnited.UnityXState.Editor {

    public static class EditorUtils {

        const string ARRAY_DATA = "Array.data[";
        const string BRACKET = "[";
        const char BRACKET_CHAR = '[';

        const BindingFlags ALL_INSTANCE_FIELDS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// Get target object reference for the SerializedProperty itself.
        /// Assumes that prop is actually a reference type.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static object GetTargetObjectOfProperty(SerializedProperty prop) {
            if (prop == null)
                return null;

            var path = prop.propertyPath.Replace(ARRAY_DATA, BRACKET);
            object obj = prop.serializedObject.targetObject;
            return GetTargetObject(obj, path);
        }

        /// <summary>
        /// Get target object containing the serialized property
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        public static object GetParentObjectOfProperty(SerializedProperty prop) {
            if (prop == null)
                return null;
            
            var propPath = prop.propertyPath;
            if (string.IsNullOrEmpty(propPath))
                return null;

            object rootTargetObj = prop.serializedObject.targetObject;
            var idx = propPath.LastIndexOf('.');
            if (idx < 0)
                // target object directly contains the property
                return rootTargetObj;

            var parentPath = propPath[..idx].Replace(ARRAY_DATA, BRACKET);
            return GetTargetObject(rootTargetObj, parentPath);
        }

        static object GetTargetObject(object target, string targetPath) {
            var targetObjType = target.GetType();
            var splitPath = targetPath.Split('.');
            var targetObj = (object) target;
            foreach (var fieldName in splitPath) {
                // if array or list index
                if (fieldName[0] == BRACKET_CHAR) {
                    try {
                        var idx = int.Parse(fieldName[1..^1]);
                        targetObj = (targetObj as IEnumerable<object>).ElementAt(idx);
                        targetObjType = targetObj.GetType();
                    } catch {
                        return null;
                    }
                }
                // if normal field
                else {
                    var field = targetObjType.GetField(fieldName, ALL_INSTANCE_FIELDS);
                    if (field == null) {
                        return null;
                    }
                    targetObj = field.GetValue(targetObj);
                    targetObjType = field.GetType();
                }
            }
            return targetObj;
        }
        
    }
}
