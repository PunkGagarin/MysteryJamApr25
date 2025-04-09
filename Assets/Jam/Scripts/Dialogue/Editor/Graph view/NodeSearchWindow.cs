using System.Collections.Generic;
using Jam.Scripts.Dialogue.Editor.Nodes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jam.Scripts.Dialogue.Editor.Graph_view
{
    public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueEditorWindow _editorWindow;
        private DialogueGraphView _graphView;

        private Texture2D _iconImage;

        public void Configure(DialogueEditorWindow editorWindow, DialogueGraphView graphView)
        {
            _editorWindow = editorWindow;
            _graphView = graphView;

            _iconImage = new Texture2D(1, 1);
            _iconImage.SetPixel(0, 0, new Color(0, 0, 0, 0));
            _iconImage.Apply();
        }
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> temp = new()
            {
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"),0),
                new SearchTreeGroupEntry(new GUIContent("Dialogue"),1),
                
                AddNodeSearch("Start", new StartNode()),
                AddNodeSearch("Dialogue", new DialogueNode()),
                AddNodeSearch("Branch", new BranchNode()),
                AddNodeSearch("Choice", new ChoiceNode()),
                AddNodeSearch("Event", new EventNode()),
                AddNodeSearch("End", new EndNode())
            };
            
            return temp;
        }

        private SearchTreeEntry AddNodeSearch(string nodeName, BaseNode baseNode) =>
            new(new GUIContent(nodeName, _iconImage))
            {
                level = 2,
                userData = baseNode
            };
            
        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Vector2 mousePos = _editorWindow.rootVisualElement.ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent, 
                context.screenMousePosition - _editorWindow.position.position);

            Vector2 graphMousePosition = _graphView.contentViewContainer.WorldToLocal(mousePos);

            return CheckForNodeType(searchTreeEntry, graphMousePosition);
        }

        private bool CheckForNodeType(SearchTreeEntry searchTreeEntry, Vector2 position)
        {
            switch (searchTreeEntry.userData)
            {
                case StartNode: 
                    _graphView.AddElement(_graphView.CreateStartNode(position));
                    return true;
                case DialogueNode: 
                    _graphView.AddElement(_graphView.CreateDialogueNode(position));
                    return true;
                case BranchNode: 
                    _graphView.AddElement(_graphView.CreateBranchNode(position));
                    return true;
                case ChoiceNode: 
                    _graphView.AddElement(_graphView.CreateChoiceNode(position));
                    return true;
                case EventNode: 
                    _graphView.AddElement(_graphView.CreateEventNode(position));
                    return true;
                case EndNode: 
                    _graphView.AddElement(_graphView.CreateEndNode(position));
                    return true;
            }

            return false;
        }
    }
}