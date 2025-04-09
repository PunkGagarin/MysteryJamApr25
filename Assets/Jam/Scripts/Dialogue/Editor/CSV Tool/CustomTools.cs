using UnityEditor;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Editor.CSV_Tool
{
    public class CustomTools
    {
        [MenuItem("Tools/Fenneig Dialogue/Save to CSV")]
        [Tooltip("Saves all dialogues in Resources/Dialogue Editor/CSV File Folder")]
        public static void SaveToCSV()
        {
            SaveCSV saveCsv = new();
            saveCsv.Save();
            
            Debug.Log("<color=green>CSV saved successfully!</color>");
        }

        [MenuItem("Tools/Fenneig Dialogue/Load from CSV")]
        [Tooltip("Load dialogues from Resources/Dialogue Editor/CSV File Folder, should have DialogueCSV_Load.csv file in there")]
        public static void LoadFromCSV()
        {
            LoadCSV loadCsv = new();
            Debug.Log(loadCsv.TryLoad()
                ? "<color=green>CSV loaded successfully!</color>"
                : "<color=red>CSV loaded unsuccessfully!</color>");
        }
    }
}
