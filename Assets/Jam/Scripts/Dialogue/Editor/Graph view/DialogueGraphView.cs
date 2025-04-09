using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Dialogue.Editor.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jam.Scripts.Dialogue.Editor.Graph_view
{
    public class DialogueGraphView : GraphView
    {
        private string _graphViewStyleSheet = "USS/GraphView/GraphViewStyleSheet";
        private DialogueEditorWindow _editorWindow;
        private NodeSearchWindow _searchWindow;

        public DialogueGraphView(DialogueEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;

            //Adding ability to zoom in and zoom out graph view.
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            StyleSheet tmpStyleSheet = Resources.Load<StyleSheet>(_graphViewStyleSheet);
            styleSheets.Add(tmpStyleSheet);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());

            GridBackground grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddSearchWindow();
        }

        private void AddSearchWindow()
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Configure(_editorWindow, this);
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }
        
        public void ReloadLanguage()
        {
            List<BaseNode> languageNodes = nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();
            foreach (BaseNode node in languageNodes)
            {
                node.ReloadLanguage();
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new();
            Port startPortView = startPort;
            ports.ForEach(port =>
            {
                Port portView = port;

                if (startPortView != portView &&
                    startPortView.node != portView.node &&
                    startPortView.direction != port.direction &&
                    startPortView.portColor == portView.portColor)
                {
                    compatiblePorts.Add(port);
                }
            });
            
            return compatiblePorts;
        }

        public StartNode CreateStartNode(Vector2 position) => new(position, _editorWindow, this);
        public DialogueNode CreateDialogueNode(Vector2 position) => new(position, _editorWindow, this);            
        public EventNode CreateEventNode(Vector2 position) => new(position, _editorWindow, this);            
        public EndNode CreateEndNode(Vector2 position) => new(position, _editorWindow, this);
        public BranchNode CreateBranchNode(Vector2 position) => new(position, _editorWindow, this);
        public ChoiceNode CreateChoiceNode(Vector2 position) => new(position, _editorWindow, this);

    }
}
