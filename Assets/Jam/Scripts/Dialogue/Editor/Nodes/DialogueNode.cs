using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Dialogue.Editor.Graph_view;
using Jam.Scripts.Dialogue.Runtime.SO.Dialogue;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jam.Scripts.Dialogue.Editor.Nodes
{
    public class DialogueNode : BaseNode
    {
        private const string DIALOGUE_NODE_STYLE_SHEET = "USS/Nodes/DialogueNodeStyleSheet";
        private List<Box> _boxes = new();
        private Toggle _skipContinueToggle;
        private bool _skipContinue;
        public DialogueData DialogueData { get; } = new();

        public bool SkipContinue
        {
            get => _skipContinue;
            set
            {
                _skipContinue = value;
                if (_skipContinueToggle != null)
                    _skipContinueToggle.value = value;
            }
        }

        public DialogueNode(){}

        public DialogueNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView, DIALOGUE_NODE_STYLE_SHEET)
        {
            title = "Dialogue";
            
            AddInputPort("Input");
            AddOutputPort("Continue");
            
            TopContainer();
            AddToggle();
        }

        private void AddToggle()
        {
            _skipContinueToggle = new Toggle("Auto Continue")
            {
                value = SkipContinue
            };
            _skipContinueToggle.RegisterValueChangedCallback(evt =>
            {
                SkipContinue = evt.newValue;
            });

            mainContainer.Add(_skipContinueToggle);
        }

        private void TopContainer()
        {
            AddPortButton();
            AddDropdownMenu();
        }

        private void AddPortButton()
        {
            Button addChoiceButton = new(){text = "Add choice"};
            addChoiceButton.clicked += () =>
            {
                AddChoicePort(this);
            };
            addChoiceButton.AddToClassList("TextBtn");
            
            titleButtonContainer.Add(addChoiceButton);

        }

        public Port AddChoicePort(BaseNode baseNode, DialogueDataPort dialogueDataPort = null)
        {
            Port port = GetPortInstance(Direction.Output);
            DialogueDataPort newDialoguePort = new();
            if (dialogueDataPort != null)
            {
                newDialoguePort.InputGuid = dialogueDataPort.InputGuid;
                newDialoguePort.OutputGuid = dialogueDataPort.OutputGuid;
                newDialoguePort.PortGuid = dialogueDataPort.PortGuid;
            }
            else
            {
                newDialoguePort.PortGuid = Guid.NewGuid().ToString();
            }

            Button removeButton = new Button(() => DeletePort(baseNode, port)) {text = "X"};
            port.contentContainer.Add(removeButton);

            port.portName = newDialoguePort.PortGuid;
            Label portNameLabel = port.contentContainer.Q<Label>("type");
            portNameLabel.AddToClassList("PortName");

            port.portColor = Color.yellow;

            DialogueData.DialogueDataPorts.Add(newDialoguePort);

            baseNode.outputContainer.Add(port);

            baseNode.RefreshPorts();
            baseNode.RefreshExpandedState();

            return port;
        }

        private void DeletePort(BaseNode baseNode, Port port)
        {
            DialogueDataPort portToRemove =
                DialogueData.DialogueDataPorts.Find(findPort => findPort.PortGuid == port.portName);
            DialogueData.DialogueDataPorts.Remove(portToRemove);

            IEnumerable<Edge> portEdges = GraphView.edges.ToList().Where(edge => edge.output == port);

            if (portEdges.Any())
            {
                Edge edge = portEdges.First();
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                GraphView.RemoveElement(edge);
            }

            baseNode.outputContainer.Remove(port);

            baseNode.RefreshPorts();
            baseNode.RefreshExpandedState();
        }

        private void AddDropdownMenu()
        {
            ToolbarMenu menu = new() {text = "Add content"};
            
            menu.menu.AppendAction("Text", _ => AddTextLine());
            menu.menu.AppendAction("Image", _ => AddImageContainer());
            menu.menu.AppendAction("Name", _ => AddCharacterName());

            titleButtonContainer.Add(menu);
        }

        public void AddTextLine(DialogueDataText dataText = null)
        {
            DialogueDataText newDialogueDataText = new();
            DialogueData.DialogueDataBaseContainers.Add(newDialogueDataText);
            
            Box boxContainer = new();
            boxContainer.AddToClassList("DialogueBox");

            AddLabelAndButtons(newDialogueDataText, boxContainer, "Text", "TextColor");
            AddTextField(newDialogueDataText, boxContainer);
            AddAudioClips(newDialogueDataText, boxContainer);

            if (dataText != null)
            {
                newDialogueDataText.GuidID = dataText.GuidID;
                
                dataText.Text.ForEach(textInData =>
                {
                    newDialogueDataText.Text.ForEach(textInContainer =>
                    {
                        if (textInContainer.LanguageType == textInData.LanguageType)
                        {
                            textInContainer.LanguageGenericType = textInData.LanguageGenericType;
                        }
                    });
                });
                
                dataText.AudioClips.ForEach(clipInData =>
                {
                    newDialogueDataText.AudioClips.ForEach(clipInContainer =>
                    {
                        if (clipInContainer.LanguageType == clipInData.LanguageType)
                        {
                            clipInContainer.LanguageGenericType = clipInData.LanguageGenericType;
                        }
                    });
                });
            }
            else
            {
                newDialogueDataText.GuidID.Value = Guid.NewGuid().ToString();
            }
            
            ReloadLanguage();
            mainContainer.Add(boxContainer);
        }

        private void AddLabelAndButtons(DialogueDataBaseContainer container, Box boxContainer, string labelName, string uniqueUSS = "")
        {
            Box topBoxContainer = new();
            topBoxContainer.AddToClassList("TopBox");

            Label labelTexts = GetNewLabel(labelName, "LabelText", uniqueUSS);
          
            Box buttonsBox = new();
            buttonsBox.AddToClassList("ButtonBox");

            Button moveUpButton = GetNewButton("", "MoveUpButton");
            moveUpButton.clicked += () =>
            {
                MoveBox(container, true);
            };

            Button moveDownButton = GetNewButton("", "MoveDownButton");
            moveDownButton.clicked += () =>
            {
                MoveBox(container, false);
            };
            
            Button removeButton = GetNewButton("X", "RemoveButton");
            removeButton.clicked += () =>
            {
                DeleteBox(boxContainer);
                _boxes.Remove(boxContainer);
                DialogueData.DialogueDataBaseContainers.Remove(container);
            };

            _boxes.Add(boxContainer);

            buttonsBox.Add(moveUpButton);
            buttonsBox.Add(moveDownButton);
            buttonsBox.Add(removeButton);
            topBoxContainer.Add(labelTexts);
            topBoxContainer.Add(buttonsBox);


            boxContainer.Add(topBoxContainer);
        }

        private void MoveBox(DialogueDataBaseContainer container, bool isMoveUp)
        {
            List<DialogueDataBaseContainer> dialogueContainer = new();
            dialogueContainer.AddRange(DialogueData.DialogueDataBaseContainers);
            
            _boxes.ForEach(box => mainContainer.Remove(box));
            
            _boxes.Clear();

            for (int i = 0; i < dialogueContainer.Count; i++)
                dialogueContainer[i].ID.Value = i;

            if (container.ID.Value > 0 && isMoveUp)
                (dialogueContainer[container.ID.Value], dialogueContainer[container.ID.Value - 1]) = (dialogueContainer[container.ID.Value - 1], dialogueContainer[container.ID.Value]);

            if (container.ID.Value < dialogueContainer.Count - 1 && !isMoveUp)
                (dialogueContainer[container.ID.Value], dialogueContainer[container.ID.Value + 1]) = (dialogueContainer[container.ID.Value + 1], dialogueContainer[container.ID.Value]);
            
            DialogueData.DialogueDataBaseContainers.Clear();

            foreach (var data in dialogueContainer)
            {
                switch (data)
                {
                    case DialogueDataName dialogueDataName:
                        AddCharacterName(dialogueDataName);
                        break;
                    case DialogueDataText dialogueDataText:
                        AddTextLine(dialogueDataText);
                        break;
                    case DialogueDataImage dialogueDataImage:
                        AddImageContainer(dialogueDataImage);
                        break;
                }
            }
        }

        private void AddTextField(DialogueDataText container, Box boxContainer)
        {
            TextField textField = GetNewTextFieldTextLanguage(container.Text, "Text area", "TextBox");

            container.TextField = textField;

            boxContainer.Add(textField);
        }
        
        private void AddTextField(DialogueDataName container, Box boxContainer)
        {
            TextField textField = GetNewTextFieldTextLanguage(container.CharacterName, "Text area", "TextBox");

            container.TextField = textField;

            boxContainer.Add(textField);
        }

        private void AddAudioClips(DialogueDataText container, Box boxContainer)
        {
            ObjectField objectField = GetNewObjectFieldAudioClipsLanguage(container.AudioClips, "AudioClip");

            container.ObjectField = objectField;

            boxContainer.Add(objectField);
        }

        public void AddImageContainer(DialogueDataImage dataImage = null)
        {
            DialogueDataImage dialogueDataImage = new();
            if (dataImage != null)
            {
                dialogueDataImage.LeftSprite = dataImage.LeftSprite;
                dialogueDataImage.RightSprite = dataImage.RightSprite;
            }

            DialogueData.DialogueDataBaseContainers.Add(dialogueDataImage);

            Box boxContainer = new();
            boxContainer.AddToClassList("DialogueBox");
            
            AddLabelAndButtons(dialogueDataImage, boxContainer, "Image", "ImageColor");
            AddImages(dialogueDataImage, boxContainer);
            
            mainContainer.Add(boxContainer);
        }

        private void AddImages(DialogueDataImage container, Box boxContainer)
        {
            Box imagePreviewBox = new();
            Box imagesBox = new();
            
            imagePreviewBox.AddToClassList("BoxRow");
            imagesBox.AddToClassList("BoxRow");

            Image leftImage = GetNewImage("ImagePreview", "ImagePreviewLeft");
            Image rightImage = GetNewImage("ImagePreview", "ImagePreviewRight");

            imagePreviewBox.Add(leftImage);
            imagePreviewBox.Add(rightImage);

            ObjectField leftObjectField = GetNewObjectFieldSprite(container.LeftSprite, leftImage, "SpriteLeft");
            ObjectField rightObjectField = GetNewObjectFieldSprite(container.RightSprite, rightImage, "SpriteRight");

            imagesBox.Add(leftObjectField);
            imagesBox.Add(rightObjectField);

            boxContainer.Add(imagePreviewBox);
            boxContainer.Add(imagesBox);
        }

        public void AddCharacterName(DialogueDataName dataName = null)
        {
            DialogueDataName newDialogueDataName = new();
            DialogueData.DialogueDataBaseContainers.Add(newDialogueDataName);

            Box boxContainer = new();
            boxContainer.AddToClassList("CharacterNameBox");
            
            AddLabelAndButtons(newDialogueDataName, boxContainer, "Name", "NameColor");
            AddTextFieldCharacterName(newDialogueDataName, boxContainer);
            
            if (dataName != null)
            {
                newDialogueDataName.GuidID.Value = dataName.GuidID.Value;
                dataName.CharacterName.ForEach(nameInData =>
                {
                    newDialogueDataName.CharacterName.ForEach(nameInContainer =>
                    {
                        if (nameInContainer.LanguageType == nameInData.LanguageType)
                        {
                            nameInContainer.LanguageGenericType = nameInData.LanguageGenericType;
                        }
                    });
                });
            }
            else
            {
                newDialogueDataName.GuidID.Value = Guid.NewGuid().ToString();
            }

            ReloadLanguage();
            mainContainer.Add(boxContainer);
        }

        private void AddTextFieldCharacterName(DialogueDataName container, Box boxContainer)
        {
            TextField textField = GetNewTextFieldTextLanguage(container.CharacterName, "Name", "CharacterName");

            container.TextField = textField;
            
            boxContainer.Add(textField);
        }
    }
}