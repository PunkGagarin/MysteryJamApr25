using Jam.Scripts.Dialogue.Editor.Graph_view;
using Jam.Scripts.Dialogue.Runtime.SO.Dialogue;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Events;
using UnityEditor.UIElements;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Editor.Nodes
{
    public class BranchNode : BaseNode
    {
        private const string BRANCH_NODE_STYLE_SHEET = "USS/Nodes/BranchNodeStyleSheet";

        public BranchData BranchData { get; set; } = new();

        public BranchNode() { }

        public BranchNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView dialogueGraphView) : base(position, editorWindow, dialogueGraphView, BRANCH_NODE_STYLE_SHEET)
        {
            title = "Branch";
            
            AddInputPort("Input");
            AddOutputPort("True");
            AddOutputPort("False");

            TopButton();
        }

        private void TopButton()
        {
            ToolbarMenu menu = new() {text = "Add condition"};
            
            menu.menu.AppendAction("String condition", _ => AddCondition());
            
            titleButtonContainer.Add(menu);
        }

        public void AddCondition(EventDataStringCondition stringEvent = null)
        {
           AddStringConditionEventBuild(BranchData.EventDataStringConditions, stringEvent);
        }
    }
}