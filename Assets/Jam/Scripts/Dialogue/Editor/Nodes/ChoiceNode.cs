using Jam.Scripts.Dialogue.Editor.Graph_view;
using Jam.Scripts.Dialogue.Runtime.SO.Dialogue;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Events;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jam.Scripts.Dialogue.Editor.Nodes
{
    public class ChoiceNode : BaseNode
    {
        private const string CHOICE_NODE_STYLE_SHEET = "USS/Nodes/ChoiceNodeStyleSheet";
        public ChoiceData ChoiceData { get; set; } = new();

        private Box _choiceStateEnumBox;
        public ChoiceNode (){}

        public ChoiceNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView dialogueGraphView) : base(position, editorWindow, dialogueGraphView, CHOICE_NODE_STYLE_SHEET)
        {
            title = "Choice";
            
            Port inputPort = AddInputPort("Input");
            AddOutputPort("Output");
            
            inputPort.portColor = Color.yellow;

            TopButton();

            TextLine();
            ChoiceStateEnum();
        }

        private void TopButton()
        {
            ToolbarMenu menu = new();
            menu.text = "Add Condition";
            
            menu.menu.AppendAction("String Event Condition", _ => AddCondition());
            titleContainer.Add(menu);
        }

        private void TextLine()
        {
            Box boxContainer = new();
            boxContainer.AddToClassList("TextLineBox");

            TextField textField = GetNewTextFieldTextLanguage(ChoiceData.Text, "Choice text", "TextBox");
            ChoiceData.TextField = textField;
            boxContainer.Add(textField);

            ObjectField objectField = GetNewObjectFieldAudioClipsLanguage(ChoiceData.AudioClips, "AudioClip");
            ChoiceData.ObjectField = objectField;
            boxContainer.Add(objectField);
            
            ReloadLanguage();

            mainContainer.Add(boxContainer);

        }

        private void ChoiceStateEnum()
        {
            _choiceStateEnumBox = new Box();
            _choiceStateEnumBox.AddToClassList("BoxRow");
            ShowHideChoiceEnum();

            Label enumLabel = GetNewLabel("If the condition is not met", "ChoiceLabel");
            EnumField choiceStateEnumField = GetNewEnumFieldChoiceType(ChoiceData.ChoiceStateType, "enumHide");

            _choiceStateEnumBox.Add(choiceStateEnumField);
            _choiceStateEnumBox.Add(enumLabel);
            
            mainContainer.Add(_choiceStateEnumBox);
        }

        public void AddCondition(EventDataStringCondition stringCondition = null)
        {
            AddStringConditionEventBuild(ChoiceData.EventDataStringConditions, stringCondition);
            ShowHideChoiceEnum();
        }

        private void ShowHideChoiceEnum()
        {
            ShowHide(ChoiceData.EventDataStringConditions.Count > 0, _choiceStateEnumBox);
        }

        protected override void DeleteBox(Box boxContainer)
        {
            base.DeleteBox(boxContainer);
            ShowHideChoiceEnum();
        }

        public override void LoadValueInToField()
        {
            ChoiceData.ChoiceStateType.EnumField?.SetValueWithoutNotify(ChoiceData.ChoiceStateType.Value);
        }
    }
}