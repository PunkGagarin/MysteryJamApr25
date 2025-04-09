using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Editor.String_Tool
{
    public class StringEventDefinition : ScriptableObject
    {
        [SerializeField] private List<string> _stringEventsForEditor = new ();
        public List<string> StringEventsForEditor => _stringEventsForEditor;

        private static StringEventDefinition _instance;

        public static StringEventDefinition I => _instance == null ? LoadDef() : _instance;

        private static StringEventDefinition LoadDef()
        {
            _instance = Resources.Load<StringEventDefinition>("String event definition");

            if (_instance == null)
            {
                _instance = CreateInstance<StringEventDefinition>();

                AssetDatabase.CreateAsset(_instance, "Assets/Resources/String event definition.asset");

                _instance._stringEventsForEditor = new List<string>();
                
                AssetDatabase.SaveAssets();
            }

            return _instance;
        }
    }
}