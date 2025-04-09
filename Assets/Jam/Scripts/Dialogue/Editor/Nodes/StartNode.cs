using Jam.Scripts.Dialogue.Editor.Graph_view;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Editor.Nodes
{
    public class StartNode : BaseNode
    {
        private const string START_NODE_STYLE_SHEET = "USS/Nodes/StartNodeStyleSheet";
        public StartNode() { }

        public StartNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView, START_NODE_STYLE_SHEET)
        {
            title = "Start";

            AddOutputPort("Output");
            
            RefreshExpandedState();
            RefreshPorts();
        }
    }
}