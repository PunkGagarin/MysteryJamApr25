﻿using System;
using System.Collections.Generic;
using System.IO;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.Dialogue.Runtime.SO;
using Jam.Scripts.Dialogue.Runtime.SO.Dialogue;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Editor.CSV_Tool
{
    public class LoadCSV
    {
        private string _csvDirectoryName = "Resources/Dialogue Editor/CSV File";
        private string _csvFileName = "DialogueCSV_Load.csv";

        private CSVReader _csvReader = new();

        public bool TryLoad()
        {
            try
            {
                string text = File.ReadAllText($"{Application.dataPath}/{_csvDirectoryName}/{_csvFileName}");
                List<List<string>> table = _csvReader.ParseCSV(text);

                List<string> headers = table[0];

                List<DialogueContainerSO> dialogueContainers = Helper.FindAllDialogueContainers();

                dialogueContainers.ForEach(dialogueContainer =>
                {
                    dialogueContainer.DialogueData.ForEach(nodeData =>
                    {
                        nodeData.DialogueDataTexts.ForEach(textData =>
                        {
                            LoadIntoDialogueNodeText(table, headers, textData);
                        });
                        
                        nodeData.DialogueDataNames.ForEach(nameData =>
                        {
                            LoadIntoNameNodeText(table, headers, nameData);
                        });
                    });

                    dialogueContainer.ChoiceData.ForEach(nodeData => { LoadIntoChoiceNode(table, headers, nodeData); });
                });
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        private void LoadIntoDialogueNodeText(List<List<string>> table, List<string> headers, DialogueDataText textData)
        {
            table.ForEach(line =>
            {
                if (line[2] == textData.GuidID.Value)
                {
                    for (int i = 0; i < line.Count; i++)
                    {
                        foreach (var languageType in (LanguageType[]) Enum.GetValues(typeof(LanguageType)))
                        {
                            if (headers[i] == languageType.ToString())
                            {
                                textData.Text.Find(x => x.LanguageType == languageType).LanguageGenericType = line[i];
                            }
                        }
                    }
                }
            });
        }

        private void LoadIntoNameNodeText(List<List<string>> table, List<string> headers, DialogueDataName nameData)
        {
            table.ForEach(line =>
            {
                if (line[2] == nameData.GuidID.Value)
                {
                    for (int i = 0; i < line.Count; i++)
                    {
                        foreach (var languageType in (LanguageType[]) Enum.GetValues(typeof(LanguageType)))
                        {
                            if (headers[i] == languageType.ToString())
                            {
                                nameData.CharacterName.Find(x => x.LanguageType == languageType).LanguageGenericType = line[i];
                            }
                        }
                    }
                }
            });
        }

        private void LoadIntoChoiceNode(List<List<string>> result, List<string> headers, ChoiceData nodeData)
        {
            result.ForEach(line =>
            {
                if (line[1] == nodeData.NodeGuid)
                {
                    for (int i = 0; i < line.Count; i++)
                    {
                        foreach (var languageType in (LanguageType[]) Enum.GetValues(typeof(LanguageType)))
                        {
                            if (headers[i] == languageType.ToString())
                            {
                                nodeData.Text.Find(x => x.LanguageType == languageType).LanguageGenericType = line[i];
                            }
                        }
                    }
                }
            });
        }
    }
}