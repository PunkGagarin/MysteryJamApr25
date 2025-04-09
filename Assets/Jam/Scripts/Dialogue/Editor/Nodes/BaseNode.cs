using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Dialogue.Editor.Graph_view;
using Jam.Scripts.Dialogue.Editor.String_Tool;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.Dialogue.Runtime.Events;
using Jam.Scripts.Dialogue.Runtime.Events.SO;
using Jam.Scripts.Dialogue.Runtime.SO;
using Jam.Scripts.Dialogue.Runtime.SO.Values;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Enums;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Events;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jam.Scripts.Dialogue.Editor.Nodes
{
    public class BaseNode : Node
    {
        private string _nodeGuid;
        protected DialogueGraphView GraphView;
        protected DialogueEditorWindow EditorWindow;
        protected static readonly Vector2 DefaultNodeSize = new(200, 250);

        private List<LanguageGenericHolderText> _languageGenericListTexts = new();
        private List<LanguageGenericHolderAudioClip> _languageGenericListAudioClips = new();

        public string NodeGuid
        {
            get => _nodeGuid;
            set => _nodeGuid = value;
        }

        public BaseNode()
        {
        }

        public BaseNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView, string specificNodeStyleSheetPath = null)
        {
            EditorWindow = editorWindow;
            GraphView = graphView;

            SetPosition(new Rect(position, DefaultNodeSize));
            NodeGuid = Guid.NewGuid().ToString();
            
            StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/NodeStyleSheet");
            styleSheets.Add(styleSheet);
            if (specificNodeStyleSheetPath != null)
            {
                StyleSheet specificNodeStyleSheet = Resources.Load<StyleSheet>(specificNodeStyleSheetPath);
                styleSheets.Add(specificNodeStyleSheet);
            }
        }

        #region Get New Field

        /// <summary>
        /// Get a new label.
        /// </summary>
        /// <param name="labelName">Text in the label.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">Uss class add to the UI element.</param>
        protected Label GetNewLabel(string labelName, string USS01 = "", string USS02 = "")
        {
            Label labelText = new Label(labelName);
            
            labelText.AddToClassList(USS01);
            labelText.AddToClassList(USS02);

            return labelText;
        }

        /// <summary>
        /// Get a new button.
        /// </summary>
        /// <param name="buttonText">Text in the button.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">Uss class add to the UI element.</param>
        protected Button GetNewButton(string buttonText, string USS01 = "", string USS02 = "")
        {
            Button button = new Button {text = buttonText};
            
            button.AddToClassList(USS01);
            button.AddToClassList(USS02);

            return button;
        }

        /// <summary>
        /// Get a new Integer field.
        /// </summary>
        /// <param name="inputValue">ContainerInt that need to be set into the InterField.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected IntegerField GetNewIntegerField(ContainerInt inputValue, string USS01 = "", string USS02 = "")
        {
            IntegerField integerField = new();

            integerField.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = value.newValue;
            });
            integerField.SetValueWithoutNotify(inputValue.Value);
            
            integerField.AddToClassList(USS01);
            integerField.AddToClassList(USS02);
            
            return integerField;
        }

        /// <summary>
        /// Get a new Float field.
        /// </summary>
        /// <param name="inputValue">ContainerFloat that need to be set into the FloatField.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected FloatField GetNewFloatField(ContainerFloat inputValue, string USS01 = "", string USS02 = "")
        {
            FloatField floatField = new();

            floatField.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = value.newValue;
            });
            floatField.SetValueWithoutNotify(inputValue.Value);
            
            floatField.AddToClassList(USS01);
            floatField.AddToClassList(USS02);
            
            return floatField;
        }

        /// <summary>
        /// Get a new Text field.
        /// </summary>
        /// <param name="inputValue">ContainerString that need to be set into the TextField.</param>
        /// <param name="placeholderText">The text that will be displayed if the text field is empty.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected TextField GetNewTextField(ContainerString inputValue, string placeholderText, string USS01 = "", string USS02 = "")
        {
            TextField textField = new();

            textField.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = value.newValue;
            });
            textField.SetValueWithoutNotify(inputValue.Value);
            
            textField.AddToClassList(USS01);
            textField.AddToClassList(USS02);
            
            SetPlaceholderText(textField, placeholderText);
            
            return textField;
        }

        /// <summary>
        /// Get a new Text field.
        /// </summary>
        /// <param name="inputValue">ContainerString that need to be set into the TextField.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected PopupField<string> GetNewEventTextField(ContainerEventString inputValue, string USS01 = "", string USS02 = "")
        {
            var eventStringSO = StringEventDefinition.I.StringEventsForEditor;

            if (eventStringSO.Count == 0)
            {
                eventStringSO.Add("Empty");
            }

            PopupField<string> dropdownMenu = new PopupField<string>(eventStringSO.ToList(), 0);

            dropdownMenu.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = value.newValue;
            });
            dropdownMenu.SetValueWithoutNotify(inputValue.Value);
            
            dropdownMenu.AddToClassList(USS01);
            dropdownMenu.AddToClassList(USS02);
            
            return dropdownMenu;
        }

        /// <summary>
        /// Get a new Image.
        /// </summary>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected Image GetNewImage(string USS01 = "", string USS02 = "")
        {
            Image image = new();
            
            image.AddToClassList(USS01);
            image.AddToClassList(USS02);

            return image;
        }

        /// <summary>
        /// Get a new ObjectField with a Sprite as the Object.
        /// </summary>
        /// <param name="inputValue">ContainerSprite that need to be set into the ObjectField.</param>
        /// <param name="imagePreview">Image that need to be set as preview image.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected ObjectField GetNewObjectFieldSprite(ContainerSprite inputValue, Image imagePreview, string USS01 = "", string USS02 = "")
        {
            ObjectField objectField = new()
            {
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = inputValue.Value
            };

            objectField.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = value.newValue as Sprite;

                imagePreview.image = inputValue.Value == null ? null : inputValue.Value.texture;
            });
            imagePreview.image = inputValue.Value == null ? null : inputValue.Value.texture;
            
            objectField.AddToClassList(USS01);
            objectField.AddToClassList(USS02);
            
            return objectField;
        }

        /// <summary>
        /// Get a new ObjectField with a ContainerDialogueEventSO as the object.
        /// </summary>
        /// <param name="inputValue">ContainerFloat that need to be set into the FloatField.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected ObjectField GetNewObjectFieldDialogueEventSO(ContainerDialogueEventSO inputValue, string USS01 = "", string USS02 = "")
        {
            ObjectField objectField = new()
            {
                objectType = typeof(DialogueEventSO),
                allowSceneObjects = false,
                value = inputValue.DialogueEventSO
            };

            objectField.RegisterValueChangedCallback(value =>
            {
                inputValue.DialogueEventSO = value.newValue as DialogueEventSO;
            });
            objectField.SetValueWithoutNotify(inputValue.DialogueEventSO);
            
            objectField.AddToClassList(USS01);
            objectField.AddToClassList(USS02);
            
            return objectField;
        }

        /// <summary>
        /// Get a new EnumField Where the enum is ChoiceStateType.
        /// </summary>
        /// <param name="inputValue">ContainerChoiceStateType that need to be set into the EnumField.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected EnumField GetNewEnumFieldChoiceType(ContainerChoiceStateType inputValue, string USS01 = "", string USS02 = "")
        {
            EnumField enumField = new()
            {
                value = inputValue.Value
            };
            enumField.Init(inputValue.Value);

            enumField.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = (ChoiceStateType) value.newValue;
            });
            enumField.SetValueWithoutNotify(inputValue.Value);
            
            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);

            inputValue.EnumField = enumField;
            return enumField;
        }

        /// <summary>
        /// Get a new EnumField Where the enum is StringEventModifierType.
        /// </summary>
        /// <param name="inputValue">ContainerStringEventModifierType that need to be set into the EnumField.</param>
        /// <param name="action">An action that called when enum field has changed</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected EnumField GetNewEnumFieldStringEventModifierType(ContainerStringEventModifierType inputValue, Action action, string USS01 = "", string USS02 = "")
        {
            EnumField enumField = new()
            {
                value = inputValue.Value
            };
            enumField.Init(inputValue.Value);

            enumField.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = (StringEventModifierType) value.newValue;
                action?.Invoke();
            });
            enumField.SetValueWithoutNotify(inputValue.Value);
            
            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);

            inputValue.EnumField = enumField;
            return enumField;
        }

        /// <summary>
        /// Get a new EnumField Where the enum is StringEventConditionType.
        /// </summary>
        /// <param name="inputValue">ContainerStringEventConditionType that need to be set into the EnumField.</param>
        /// <param name="action">An action that is use to hide/show depending on if a FloatField is needed</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected EnumField GetNewEnumFieldStringEventConditionType(ContainerStringEventConditionType inputValue, Action action, string USS01 = "", string USS02 = "")
        {
            EnumField enumField = new()
            {
                value = inputValue.Value
            };
            enumField.Init(inputValue.Value);

            enumField.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = (StringEventConditionType) value.newValue;
                action?.Invoke();
            });
            enumField.SetValueWithoutNotify(inputValue.Value);
            
            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);

            inputValue.EnumField = enumField;
            return enumField;
        }

        /// <summary>
        /// Get a new EnumField Where the enum is StringEventConditionType.
        /// </summary>
        /// <param name="inputValue">ContainerStringEventConditionType that need to be set into the EnumField.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected EnumField GetNewEnumFieldEndNodeType(ContainerEndNodeType inputValue, string USS01 = "", string USS02 = "")
        {
            EnumField enumField = new()
            {
                value = inputValue.Value
            };
            enumField.Init(inputValue.Value);

            enumField.RegisterValueChangedCallback(value =>
            {
                inputValue.Value = (EndNodeType) value.newValue;
            });
            enumField.SetValueWithoutNotify(inputValue.Value);
            
            enumField.AddToClassList(USS01);
            enumField.AddToClassList(USS02);

            inputValue.EnumField = enumField;
            return enumField;
        }

        /// <summary>
        /// Get a new TextField that use a List<LanguageGeneric<string>> text.
        /// </summary>
        /// <param name="inputValue">List of LanguageGeneric<string> Text.</param>
        /// <param name="placeholder">The text that will be displayed if the text field is empty.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected TextField GetNewTextFieldTextLanguage(List<LanguageGeneric<string>> inputValue, string placeholder, string USS01 = "", string USS02 = "")
        {
            inputValue.AddRange(((LanguageType[]) Enum.GetValues(typeof(LanguageType)))
                .Select(language => new LanguageGeneric<string>
                {
                    LanguageType = language, 
                    LanguageGenericType = ""
                }));

            TextField textField = new("");

            _languageGenericListTexts.Add(new LanguageGenericHolderText(inputValue, textField, placeholder));

            textField.RegisterValueChangedCallback(value =>
            {
                inputValue.Find(text => text.LanguageType == EditorWindow.CurrentLanguage).LanguageGenericType = value.newValue;
            });
            textField.SetValueWithoutNotify(inputValue.Find(languageGenericText => languageGenericText.LanguageType == EditorWindow.CurrentLanguage).LanguageGenericType);

            textField.multiline = true;
            
            textField.AddToClassList(USS01);
            textField.AddToClassList(USS02);
            
            return textField;
        }

        /// <summary>
        /// Get a new ObjectField that use a List<LanguageGeneric<AudioClip>> text.
        /// </summary>
        /// <param name="inputValue">List of LanguageGeneric<AudioClip> Text.</param>
        /// <param name="USS01">USS class add to the UI element.</param>
        /// <param name="USS02">USS class add to the UI element.</param>
        protected ObjectField GetNewObjectFieldAudioClipsLanguage(List<LanguageGeneric<AudioClip>> inputValue, string USS01 = "", string USS02 = "")
        {
            inputValue.AddRange(((LanguageType[]) Enum.GetValues(typeof(LanguageType)))
                .Select(language => new LanguageGeneric<AudioClip>
                {
                    LanguageType = language, 
                    LanguageGenericType = null
                }));

            ObjectField objectField = new()
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = inputValue.Find(text => text.LanguageType == EditorWindow.CurrentLanguage).LanguageGenericType
            };

            _languageGenericListAudioClips.Add(new LanguageGenericHolderAudioClip(inputValue, objectField));

            objectField.RegisterValueChangedCallback(value =>
            {
                inputValue.Find(clip => clip.LanguageType == EditorWindow.CurrentLanguage).LanguageGenericType = value.newValue as AudioClip;
            });
            objectField.SetValueWithoutNotify(inputValue.Find(languageGenericText => languageGenericText.LanguageType == EditorWindow.CurrentLanguage).LanguageGenericType);

            objectField.AddToClassList(USS01);
            objectField.AddToClassList(USS02);
            
            return objectField;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add String Modifier Event to UI element.
        /// </summary>
        /// <param name="stringEventModifier">The List<EventDataStringModifier> that EventDataStringModifier should be added to.</param>
        /// <param name="stringEvent">EventDataStringModifier that should be use.</param>
        protected void AddStringModifierEventBuild(List<EventDataStringModifier> stringEventModifier, EventDataStringModifier stringEvent = null)
        {
            EventDataStringModifier tempStringModifier = new();

            if (stringEvent != null)
            {
                tempStringModifier.StringEventText.Value = stringEvent.StringEventText.Value;
                tempStringModifier.Number.Value = stringEvent.Number.Value;
                tempStringModifier.StringEventModifierType.Value = stringEvent.StringEventModifierType.Value;
            }

            stringEventModifier.Add(tempStringModifier);

            Box boxContainer = new();
            Box boxFloatField = new();
            boxContainer.AddToClassList("StringEventBox");
            boxFloatField.AddToClassList("StringEventBoxFloatField");

            PopupField<string> textField = GetNewEventTextField(tempStringModifier.StringEventText, "StringEventText");
            FloatField floatField = GetNewFloatField(tempStringModifier.Number, "StringEventInt");

            Action ShowHideAction = () => ShowHideStringEventModifierType(tempStringModifier.StringEventModifierType.Value, boxFloatField);
            EnumField enumField = GetNewEnumFieldStringEventModifierType(tempStringModifier.StringEventModifierType, ShowHideAction, "StringEventEnum");
            ShowHideStringEventModifierType(tempStringModifier.StringEventModifierType.Value, boxFloatField);

            Button removeButton = GetNewButton("X", "RemoveButton");
            removeButton.clicked += () =>
            {
                stringEventModifier.Remove(tempStringModifier);
                DeleteBox(boxContainer);
            };

            boxContainer.Add(textField);
            boxContainer.Add(enumField);
            boxFloatField.Add(floatField);
            boxContainer.Add(boxFloatField);
            boxContainer.Add(removeButton);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }
        
        /// <summary>
        /// Add String Condition Event to UI element.
        /// </summary>
        /// <param name="stringEventCondition">The List<EventDataStringCondition> that EventDataStringCondition should be added to.</param>
        /// <param name="stringEvent">EventDataStringCondition that should be use.</param>
        protected void AddStringConditionEventBuild(List<EventDataStringCondition> stringEventCondition, EventDataStringCondition stringEvent = null)
        {
            EventDataStringCondition tempStringCondition = new();

            if (stringEvent != null)
            {
                tempStringCondition.StringEventText.Value = stringEvent.StringEventText.Value;
                tempStringCondition.Number.Value = stringEvent.Number.Value;
                tempStringCondition.StringEventConditionType.Value = stringEvent.StringEventConditionType.Value;
            }

            stringEventCondition.Add(tempStringCondition);

            Box boxContainer = new();
            Box boxFloatField = new();
            boxContainer.AddToClassList("StringEventBox");
            boxFloatField.AddToClassList("StringEventBoxFloatField");
            
            PopupField<string> textField = GetNewEventTextField(tempStringCondition.StringEventText, "StringEventText");
            FloatField floatField = GetNewFloatField(tempStringCondition.Number, "StringEventInt");

            Action ShowHideAction = () => ShowHideStringEventConditionType(tempStringCondition.StringEventConditionType.Value, boxFloatField);
            EnumField enumField = GetNewEnumFieldStringEventConditionType(tempStringCondition.StringEventConditionType, ShowHideAction, "StringEventEnum");
            ShowHideStringEventConditionType(tempStringCondition.StringEventConditionType.Value, boxFloatField);

            Button removeButton = GetNewButton("X", "RemoveButton");
            removeButton.AddToClassList("RemoveButton");

            removeButton.clicked += () =>
            {
                stringEventCondition.Remove(tempStringCondition);
                DeleteBox(boxContainer);
            };

            boxContainer.Add(textField);
            boxContainer.Add(enumField);
            boxFloatField.Add(floatField);
            boxContainer.Add(boxFloatField);
            boxContainer.Add(removeButton);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }

        /// <summary>
        /// Remove box container from main box.
        /// </summary>
        /// <param name="boxContainer">Desired box to remove</param>
        protected virtual void DeleteBox(Box boxContainer)
        {   
            mainContainer.Remove(boxContainer);
            RefreshExpandedState();
        }

        /// <summary>
        /// Hide and show the UI element
        /// </summary>
        /// <param name="value">new StringEventModifierType value</param>
        /// <param name="boxContainer">The Box that will be hidden or shown</param>
        private void ShowHideStringEventModifierType(StringEventModifierType value, Box boxContainer) =>
            ShowHide(value is not (StringEventModifierType.SetTrue or StringEventModifierType.SetFalse), boxContainer);

        /// <summary>
        /// Hide and show the UI element
        /// </summary>
        /// <param name="value">new StringEventModifierType value</param>
        /// <param name="boxContainer">The Box that will be hidden or shown</param>
        private void ShowHideStringEventConditionType(StringEventConditionType value, Box boxContainer) =>
            ShowHide(value is not (StringEventConditionType.True or StringEventConditionType.False), boxContainer);

        protected void ShowHide(bool isShow, Box boxContainer)
        {
            string hideUssClass = "Hide";
            if (!isShow)
            {
                boxContainer.AddToClassList(hideUssClass);
            }
            else
            {
                boxContainer.RemoveFromClassList(hideUssClass);
            }
        }
        
        /// <summary>
        /// Add a port to the outputContainer.
        /// </summary>
        /// <param name="portName">The name of port.</param>
        /// <param name="capacity">Can it accept multiple or a single one.</param>
        /// <returns>Get the port that was just added to the outputContainer.</returns>
        public Port AddOutputPort(string portName, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port outputPort = GetPortInstance(Direction.Output, capacity);
            outputPort.portName = portName;
            outputContainer.Add(outputPort);
            return outputPort;
        }

        /// <summary>
        /// Add a port to the inputContainer.
        /// </summary>
        /// <param name="portName">The name of port.</param>
        /// <param name="capacity">Can it accept multiple or a single one.</param>
        /// <returns>Get the port that was just added to the inputContainer.</returns>
        public Port AddInputPort(string portName, Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port inputPort = GetPortInstance(Direction.Input, capacity);
            inputPort.portName = portName;
            inputContainer.Add(inputPort);
            return inputPort;
        }

        /// <summary>
        /// Make a new port and return it.
        /// </summary>
        /// <param name="nodeDirection">What direction the port is input or output</param>
        /// <param name="capacity">Can it accept multiple or a single one</param>
        /// <returns>Get new port</returns>
        public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single) =>
            InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));

        public virtual void LoadValueInToField()
        {
        }

        public virtual void ReloadLanguage()
        {
            _languageGenericListTexts.ForEach(text => ReloadTextLanguage(text.InputText, text.TextField, text.PlaceholderText));
            _languageGenericListAudioClips.ForEach(clip => ReloadAudioClipLanguage(clip.InputAudioClip, clip.ObjectField));
            
        }

        /// <summary>
        /// Reload all the texts in the TextField to the current selected language.
        /// </summary>
        /// <param name="inputText">List of LanguageGeneric<string>.</param>
        /// <param name="textField">The TextField that is to be reload.</param>
        /// <param name="placeholderText">The text that will be displayed if the text field is empty.</param>
        private void ReloadTextLanguage(List<LanguageGeneric<string>> inputText, TextField textField, string placeholderText)
        {
            textField.RegisterValueChangedCallback(value =>
            {
                inputText.Find(text => text.LanguageType == EditorWindow.CurrentLanguage).LanguageGenericType = value.newValue;
            });
            textField.SetValueWithoutNotify(inputText.Find(text => text.LanguageType == EditorWindow.CurrentLanguage).LanguageGenericType);

            SetPlaceholderText(textField, placeholderText);
        }

        /// <summary>
        /// Set placeholder text on a TextField.
        /// </summary>
        /// <param name="textField">TextField that need a placeholder.</param>
        /// <param name="placeholderText">The text that will be displayed if the text field is empty.</param>
        protected void SetPlaceholderText(TextField textField, string placeholderText)
        {
            string placeholderClass = TextField.ussClassName + "__placeholder";

            CheckForText();
            OnFocusOut();
            textField.RegisterCallback<FocusInEvent>(e => OnFocusIn());
            textField.RegisterCallback<FocusOutEvent>(e => OnFocusOut());
            
            void OnFocusIn()
            {
                if (!textField.ClassListContains(placeholderClass)) return;
                
                textField.value = string.Empty;
                textField.RemoveFromClassList(placeholderClass);
            }

            void OnFocusOut()
            {
                if (!string.IsNullOrEmpty(textField.text)) return;
                
                textField.SetValueWithoutNotify(placeholderText);
                textField.AddToClassList(placeholderClass);
            }

            void CheckForText()
            {
                if (string.IsNullOrEmpty(textField.text)) return;
                
                textField.RemoveFromClassList(placeholderClass);
            }
        }
        
        /// <summary>
        /// Reload all the AudioClip in the ObjectField to the current selected language.
        /// </summary>
        /// <param name="inputAudioClip">List of LanguageGeneric<AudioClip>.</param>
        /// <param name="objectField">The ObjectField that is to be reload.</param>
        private void ReloadAudioClipLanguage(List<LanguageGeneric<AudioClip>> inputAudioClip, ObjectField objectField)
        {
            objectField.RegisterValueChangedCallback(value =>
            {
                inputAudioClip.Find(clip => clip.LanguageType == EditorWindow.CurrentLanguage).LanguageGenericType = value.newValue as AudioClip;
            });
            objectField.SetValueWithoutNotify(inputAudioClip.Find(clip => clip.LanguageType == EditorWindow.CurrentLanguage).LanguageGenericType);
        }
        #endregion

        #region LanguageGenericHolder class

        public class LanguageGenericHolderText
        {
            public List<LanguageGeneric<string>> InputText;
            public TextField TextField;
            public string PlaceholderText;
            
            public LanguageGenericHolderText(List<LanguageGeneric<string>> inputText, TextField textField, string placeholderText = "placeholder")
            {
                InputText = inputText;
                TextField = textField;
                PlaceholderText = placeholderText;
            }
        }

        public class LanguageGenericHolderAudioClip
        {
            public List<LanguageGeneric<AudioClip>> InputAudioClip;
            public ObjectField ObjectField;
            
            public LanguageGenericHolderAudioClip(List<LanguageGeneric<AudioClip>> inputAudioClip, ObjectField objectField)
            {
                InputAudioClip = inputAudioClip;
                ObjectField = objectField;
            }
        }
        #endregion
    }
}