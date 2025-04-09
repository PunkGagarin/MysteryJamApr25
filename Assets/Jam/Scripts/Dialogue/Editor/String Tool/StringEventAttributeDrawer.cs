using System.Linq;
using Jam.Scripts.Utils.String_Tool;
using UnityEditor;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Editor.String_Tool
{
    [CustomPropertyDrawer(typeof(StringEventAttribute))]
    public class StringEventAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defs = StringEventDefinition.I.StringEventsForEditor;
            var ids = defs.ToList();
            if (ids.Count == 0) return;

            var index = Mathf.Max(ids.IndexOf(property.stringValue), 0);
            index = EditorGUI.Popup(position, property.displayName, index, ids.ToArray());
            property.stringValue = ids[index];
        }
    }
}