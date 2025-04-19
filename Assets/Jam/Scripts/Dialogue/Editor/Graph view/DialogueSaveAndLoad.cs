using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Dialogue.Editor.Nodes;
using Jam.Scripts.Dialogue.Runtime.SO;
using Jam.Scripts.Dialogue.Runtime.SO.Dialogue;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Events;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace Jam.Scripts.Dialogue.Editor.Graph_view
{
    public class DialogueSaveAndLoad
    {
        private DialogueGraphView _graphView;
        private List<BaseNode> Nodes => _graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();
        private List<Edge> Edges => _graphView.edges.ToList();

        public DialogueSaveAndLoad(DialogueGraphView graphView)
        {
            _graphView = graphView;
        }

        public void Save(DialogueContainerSO dialogueContainer)
        {
            SaveEdges(dialogueContainer);
            SaveNodes(dialogueContainer);
            
            EditorUtility.SetDirty(dialogueContainer);
            AssetDatabase.SaveAssets();
        }

        public void Load(DialogueContainerSO dialogueContainer)
        {
            ClearGraph();
            GenerateNodes(dialogueContainer);
            ConnectNodes(dialogueContainer);
        }

        #region Save

        private void SaveEdges(DialogueContainerSO dialogueContainer)
        {
            dialogueContainer.NodeLinkData.Clear();

            Edge[] connectedEdges = Edges.Where(edge => edge.input.node != null).ToArray();
            foreach (var connectedEdge in connectedEdges)
            {
                BaseNode outputNode = (BaseNode)connectedEdge.output.node;
                BaseNode inputNode  = (BaseNode)connectedEdge.input.node;
                
                dialogueContainer.NodeLinkData.Add(new LinkData
                {
                    BaseNodeGuid = outputNode.NodeGuid,
                    TargetNodeGuid = inputNode.NodeGuid,
                    BasePortGuid = connectedEdge.output.portName,
                    TargetPortGuid = connectedEdge.input.portName
                });
            }
        }

        private void SaveNodes(DialogueContainerSO dialogueContainer)
        {
            dialogueContainer.ClearData();

            Nodes.ForEach(node =>
            {
                switch (node)
                {
                    case StartNode startNode:
                        dialogueContainer.StartData.Add(SaveNodeData(startNode));
                        SaveNodeData((StartNode)node);
                        break;
                    case DialogueNode dialogueNode:
                        dialogueContainer.DialogueData.Add(SaveNodeData(dialogueNode));
                        SaveNodeData((DialogueNode)node);
                        break;
                    case BranchNode branchNode:
                        dialogueContainer.BranchData.Add(SaveNodeData(branchNode));
                        SaveNodeData((BranchNode)node);
                        break;
                    case ChoiceNode choiceNode:
                        dialogueContainer.ChoiceData.Add(SaveNodeData(choiceNode));
                        SaveNodeData((ChoiceNode)node);
                        break;
                     case EventNode eventNode:
                         dialogueContainer.EventData.Add(SaveNodeData(eventNode));
                         SaveNodeData((EventNode)node);
                         break;
                    case EndNode endNode:
                        dialogueContainer.EndData.Add(SaveNodeData(endNode));
                        SaveNodeData((EndNode)node);
                        break;
                    default: 
                        break;
                }
            });
        }

        private StartData SaveNodeData(StartNode node)
        {
            StartData startNodeData = new StartData
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position
            };

            return startNodeData;
        }

        private DialogueData SaveNodeData(DialogueNode node)
        {
            DialogueData dialogueNodeData = new DialogueData
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position
            };

            for (int i = 0; i < node.DialogueData.DialogueDataBaseContainers.Count; i++)
                node.DialogueData.DialogueDataBaseContainers[i].ID.Value = i;
            
            node.DialogueData.DialogueDataBaseContainers.ForEach(baseContainer =>
            {
                switch (baseContainer)
                {
                    case DialogueDataName nameData:
                    {
                        DialogueDataName dataToAdd = new()
                        {
                            ID = { Value = nameData.ID.Value},
                            GuidID = { Value = nameData.GuidID.Value },
                            CharacterName = nameData.CharacterName
                        };

                        dialogueNodeData.DialogueDataNames.Add(dataToAdd);
                        break;
                    }
                    case DialogueDataText textData:
                    {
                        DialogueDataText dataToAdd = new()
                        {
                            ID = { Value = textData.ID.Value },
                            GuidID = { Value = textData.GuidID.Value },
                            Text = textData.Text,
                            AudioClips = textData.AudioClips
                        };

                        dialogueNodeData.DialogueDataTexts.Add(dataToAdd);
                        break;
                    }
                    case DialogueDataImage imageData:
                    {
                        DialogueDataImage dataToAdd = new()
                        {
                            ID = { Value = imageData.ID.Value },
                            LeftSprite = { Value = imageData.LeftSprite.Value },
                            RightSprite = { Value = imageData.RightSprite.Value }
                        };

                        dialogueNodeData.DialogueDataImages.Add(dataToAdd);
                        break;
                    }
                }
            });
            
            node.DialogueData.DialogueDataPorts.ForEach(port =>
            {
                DialogueDataPort portData = new();
                portData.OutputGuid = string.Empty;
                portData.InputGuid = string.Empty;
                portData.PortGuid = port.PortGuid;
                
                Edges.ForEach(edge =>
                {
                    if (edge.output.portName == port.PortGuid)
                    {
                        portData.OutputGuid = ((BaseNode)edge.output.node).NodeGuid;
                        portData.InputGuid = ((BaseNode)edge.input.node).NodeGuid;
                    }
                });
                
                dialogueNodeData.DialogueDataPorts.Add(portData);
            });
            
            return dialogueNodeData;
        }
        
        private BranchData SaveNodeData(BranchNode node)
        {
            Edge trueOutput = Edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "True");
            Edge falseOutput = Edges.FirstOrDefault(x => x.output.node == node && x.output.portName == "False");
            
            BranchData branchNodeData = new BranchData
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position,
                TrueGuidNode = trueOutput != null ? ((BaseNode) trueOutput.input.node).NodeGuid : string.Empty,
                FalseGuidNode = falseOutput != null ? ((BaseNode) falseOutput.input.node).NodeGuid : string.Empty,
            };
            
            node.BranchData.EventDataStringConditions.ForEach(stringEvent =>
            {
                EventDataStringCondition temp = new()
                {
                    Number = { Value = stringEvent.Number.Value },
                    StringEventText = { Value = stringEvent.StringEventText.Value },
                    StringEventConditionType = { Value = stringEvent.StringEventConditionType.Value}
                };

                branchNodeData.EventDataStringConditions.Add(temp);
            });

            return branchNodeData;
        }
        
        private ChoiceData SaveNodeData(ChoiceNode node)
        {
            ChoiceData choiceNodeData = new ChoiceData
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position,
                Text = node.ChoiceData.Text,
                AudioClips = node.ChoiceData.AudioClips,
                ChoiceStateType =
                {
                    Value = node.ChoiceData.ChoiceStateType.Value
                }
            };

            node.ChoiceData.EventDataStringConditions.ForEach(stringEvent =>
            {
                EventDataStringCondition temp = new()
                {
                    Number = { Value = stringEvent.Number.Value },
                    StringEventText = { Value = stringEvent.StringEventText.Value },
                    StringEventConditionType = { Value = stringEvent.StringEventConditionType.Value}
                };

                choiceNodeData.EventDataStringConditions.Add(temp);
            });

            return choiceNodeData;
        }
        
        private EventData SaveNodeData(EventNode node)
        {
            EventData eventNodeData = new EventData
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position
            };

            node.EventData.ContainerDialogueEventSOs.ForEach(dialogueEvent =>
                eventNodeData.ContainerDialogueEventSOs.Add(dialogueEvent));
            
            node.EventData.EventDataStringModifiers.ForEach(stringEvent =>
            {
                EventDataStringModifier temp = new()
                {
                    Number = { Value = stringEvent.Number.Value },
                    StringEventText = { Value = stringEvent.StringEventText.Value },
                    StringEventModifierType = { Value = stringEvent.StringEventModifierType.Value}
                };

                eventNodeData.EventDataStringModifiers.Add(temp);
            });

            return eventNodeData;
        }
        
        private EndData SaveNodeData(EndNode node)
        {
            EndData endNodeData = new EndData
            {
                NodeGuid = node.NodeGuid,
                Position = node.GetPosition().position
            };
            endNodeData.EndNodeType.Value = node.EndData.EndNodeType.Value;

            return endNodeData;
        }

        #endregion

        #region Load

        private void ClearGraph()
        {
            Edges.ForEach(edge => _graphView.RemoveElement(edge));
            
            Nodes.ForEach(node => _graphView.RemoveElement(node));
        }

        private void GenerateNodes(DialogueContainerSO dialogueContainer)
        {
            //Start nodes
            dialogueContainer.StartData.ForEach(node =>
            {
                StartNode tempNode = _graphView.CreateStartNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                
                _graphView.AddElement(tempNode);
            });
            
            //End nodes
            dialogueContainer.EndData.ForEach(node =>
            {
                EndNode tempNode = _graphView.CreateEndNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                tempNode.EndData.EndNodeType.Value = node.EndNodeType.Value;
                
                tempNode.LoadValueInToField();
                _graphView.AddElement(tempNode);
            });
            
            //Branch
            dialogueContainer.BranchData.ForEach(node =>
            {
                BranchNode tempNode = _graphView.CreateBranchNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                node.EventDataStringConditions.ForEach(item => tempNode.AddCondition(item));
                
                tempNode.LoadValueInToField();
                tempNode.ReloadLanguage();
                _graphView.AddElement(tempNode);
            });
            
            //Choice
            dialogueContainer.ChoiceData.ForEach(node =>
            {
                ChoiceNode tempNode = _graphView.CreateChoiceNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;

                tempNode.ChoiceData.ChoiceStateType.Value = node.ChoiceStateType.Value;
                
                node.Text.ForEach(dataText =>
                {
                    tempNode.ChoiceData.Text.ForEach(editorText =>
                    {
                        if (editorText.LanguageType == dataText.LanguageType)
                        {
                            editorText.LanguageGenericType = dataText.LanguageGenericType;
                        }
                    });
                });
                
                node.AudioClips.ForEach(dataClip =>
                {
                    tempNode.ChoiceData.AudioClips.ForEach(editorClip =>
                    {
                        if (editorClip.LanguageType == dataClip.LanguageType)
                        {
                            editorClip.LanguageGenericType = dataClip.LanguageGenericType;
                        }
                    });
                });
                
                node.EventDataStringConditions.ForEach(item => tempNode.AddCondition(item));
                
                tempNode.LoadValueInToField();
                tempNode.ReloadLanguage();
                _graphView.AddElement(tempNode);
            });
            
            //Events
            dialogueContainer.EventData.ForEach(node =>
            {
                EventNode tempNode = _graphView.CreateEventNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;
                
                node.ContainerDialogueEventSOs.ForEach(item => tempNode.AddScriptableEvent(item));
                node.EventDataStringModifiers.ForEach(item => tempNode.AddStringEvent(item));
                
                tempNode.LoadValueInToField();
                _graphView.AddElement(tempNode);
            });
            
            //Dialogue
            dialogueContainer.DialogueData.ForEach(node =>
            {
                DialogueNode tempNode = _graphView.CreateDialogueNode(node.Position);
                tempNode.NodeGuid = node.NodeGuid;

                List<DialogueDataBaseContainer> dataBaseContainer = new();
                
                dataBaseContainer.AddRange(node.DialogueDataImages);
                dataBaseContainer.AddRange(node.DialogueDataTexts);
                dataBaseContainer.AddRange(node.DialogueDataNames);
                
                dataBaseContainer.Sort((x, y) => x.ID.Value.CompareTo(y.ID.Value));
                
                dataBaseContainer.ForEach(data =>
                {
                    switch (data)
                    {
                        case DialogueDataImage dialogueDataImage:
                            tempNode.AddImageContainer(dialogueDataImage);
                            break;
                        case DialogueDataName dialogueDataName:
                            tempNode.AddCharacterName(dialogueDataName);
                            break;
                        case DialogueDataText dialogueDataText:
                            tempNode.AddTextLine(dialogueDataText);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(data));
                    }
                });

                node.DialogueDataPorts.ForEach(port => tempNode.AddChoicePort(tempNode, port));
                
                tempNode.LoadValueInToField();
                tempNode.ReloadLanguage();
                _graphView.AddElement(tempNode);
            });
        }

        private void ConnectNodes(DialogueContainerSO dialogueContainer)
        {
            Nodes.ForEach(node =>
            {
                List<LinkData> connections = dialogueContainer.NodeLinkData.Where(edge => edge.BaseNodeGuid == node.NodeGuid).ToList();

                List<Port> allOutputPorts = node.outputContainer.Children().Where(port => port is Port).Cast<Port>().ToList();
                
                connections.ForEach(connection =>
                {
                    string targetNodeGuid = connection.TargetNodeGuid;
                    Port targetPort = (Port) Nodes.First(tempNode => tempNode.NodeGuid == targetNodeGuid).inputContainer[0];

                    if (targetPort != null)
                    {
                        allOutputPorts.ForEach(outputPort =>
                        {
                            if (outputPort.portName == connection.BasePortGuid)
                            {
                                LinkNodes(outputPort, targetPort);
                            }
                        });
                    }
                });
            });
        }

        private void LinkNodes(Port outputPort, Port inputPort)
        {
            Edge tempEdge = new Edge
            {
                output = outputPort,
                input = inputPort
            };
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            _graphView.Add(tempEdge);
        }

        #endregion
    }
}