using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.Dialogue.Runtime.SO;
using Jam.Scripts.Dialogue.Runtime.SO.Dialogue;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Editor.CSV_Tool
{
    public class SaveCSV
    {
        private string _csvDirectoryName = "Resources/Dialogue Editor/CSV File";
        private string _csvFileName = "DialogueCSV_Save.csv";
        private string _csvSeparator = ",";
        private string[] _csvHeader;
        private string _nodeID = "Node Guid ID";
        private string _textID = "Text Guid ID";
        private string _dialogueName = "Dialogue name";

        private string DirectoryPath => $"{Application.dataPath}/{_csvDirectoryName}";

        private string FilePath => $"{DirectoryPath}/{_csvFileName}";

        public void Save()
        {
            List<DialogueContainerSO> dialogueContainers = Helper.FindAllDialogueContainers();
            
            CreateFile();

            foreach (DialogueContainerSO dialogueContainer in dialogueContainers)
            {
                foreach (DialogueData nodeData in dialogueContainer.DialogueData)
                {
                    foreach (DialogueDataText textData in nodeData.DialogueDataTexts)
                    {
                        List<string> texts = new List<string>
                        {
                            dialogueContainer.name,
                            nodeData.NodeGuid,
                            textData.GuidID.Value
                        };

                        foreach (LanguageType languageType in (LanguageType[]) Enum.GetValues(typeof(LanguageType)))
                        {
                            string tmp = textData.Text.Find(language => language.LanguageType == languageType)
                                .LanguageGenericType.Replace("\"", "\"\"");
                            texts.Add($"\"{tmp}\"");
                        }

                        AppendToFile(texts);
                    }
                    
                    foreach (DialogueDataName dataName in nodeData.DialogueDataNames)
                    {
                        List<string> texts = new List<string>
                        {
                            dialogueContainer.name,
                            nodeData.NodeGuid,
                            dataName.GuidID.Value
                        };

                        foreach (LanguageType languageType in (LanguageType[]) Enum.GetValues(typeof(LanguageType)))
                        {
                            string tmp = dataName.CharacterName.Find(language => language.LanguageType == languageType)
                                .LanguageGenericType.Replace("\"", "\"\"");
                            texts.Add($"\"{tmp}\"");
                        }

                        AppendToFile(texts);
                    }
                    
                    
                }
                foreach (ChoiceData nodeData in dialogueContainer.ChoiceData)
                {
                    List<string> texts = new List<string>
                    {
                        dialogueContainer.name,
                        nodeData.NodeGuid,
                        "Choice Dont have Text ID"
                    };

                    foreach (LanguageType languageType in (LanguageType[]) Enum.GetValues(typeof(LanguageType)))
                    {
                        string tmp = nodeData.Text.Find(language => language.LanguageType == languageType)
                            .LanguageGenericType.Replace("\"", "\"\"");
                        texts.Add($"\"{tmp}\"");
                    }

                    AppendToFile(texts);
                }
            }
        }

        private void AppendToFile(List<string> strings)
        {
            using StreamWriter sw = File.AppendText(FilePath);
            string finalString = "";
            foreach (var text in strings)
            {
                if (finalString != "")
                {
                    finalString += _csvSeparator;
                }
                finalString += text;
            }

            sw.WriteLine(finalString);
        }

        private void CreateFile()
        {
            VerifyDirectory();
            MakeHeader();
            using StreamWriter sw = File.CreateText(FilePath);
            string finalString = "";
            foreach (var header in _csvHeader)
            {
                if (finalString != "")
                {
                    finalString += _csvSeparator;
                }

                finalString += header;
            }

            sw.WriteLine(finalString);
        }

        private void MakeHeader()
        {
            List<string> headerText = new()
            {
                _dialogueName,
                _nodeID,
                _textID
            };
            headerText.AddRange(from language in (LanguageType[]) Enum.GetValues(typeof(LanguageType)) select language.ToString());

            _csvHeader = headerText.ToArray();
        }

        private void VerifyDirectory()
        {
            string directory = DirectoryPath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}