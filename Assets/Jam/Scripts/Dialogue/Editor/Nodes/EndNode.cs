using Jam.Scripts.Dialogue.Editor.Graph_view;
using Jam.Scripts.Dialogue.Runtime.SO.Dialogue;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_2022_1_OR_NEWER
#else
using UnityEditor.UIElements;
#endif

namespace Jam.Scripts.Dialogue.Editor.Nodes
{
    public class EndNode : BaseNode
    {
        private const string END_NODE_STYLE_SHEET = "USS/Nodes/EndNodeStyleSheet";
        public EndData EndData { get; set; } = new();

        public EndNode()
        {
        }

        public EndNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView, END_NODE_STYLE_SHEET)
        {
            title = "End";

            AddInputPort("Input");
            MakeMainContainer();
        }

        private void MakeMainContainer()
        {
            EnumField enumField = GetNewEnumFieldEndNodeType(EndData.EndNodeType);
            mainContainer.Add(enumField);
            RefreshExpandedState();
        }

        public override void LoadValueInToField()
        {
            if (EndData.EndNodeType.EnumField != null)
                EndData.EndNodeType.EnumField.SetValueWithoutNotify(EndData.EndNodeType.Value);
        }
    }
}