using Jam.Scripts.Dialogue.Editor.Graph_view;
using Jam.Scripts.Dialogue.Runtime.SO;
using Jam.Scripts.Dialogue.Runtime.SO.Dialogue;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Events;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jam.Scripts.Dialogue.Editor.Nodes
{
    public class EventNode : BaseNode
    {
        private const string EVENT_NODE_STYLE_SHEET = "USS/Nodes/EventNodeStyleSheet";
        public EventData EventData { get; } = new();

        public EventNode() { }

        public EventNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView) : base(position, editorWindow, graphView, EVENT_NODE_STYLE_SHEET)
        {
            title = "Event";

            AddInputPort("Input");
            AddOutputPort("Output");
            
            TopButton();
        }

        private void TopButton()
        {
            ToolbarMenu menu = new ToolbarMenu();
            menu.text = "Add event";
            
            menu.menu.AppendAction("String event modifier", _ => AddStringEvent());
            menu.menu.AppendAction("Scriptable object", _ => AddScriptableEvent());

            titleContainer.Add(menu);
        }

        public void AddStringEvent(EventDataStringModifier stringEvent = null)
        {
            AddStringModifierEventBuild(EventData.EventDataStringModifiers, stringEvent);
        }

        public void AddScriptableEvent(ContainerDialogueEventSO existContainerDialogueEvent = null)
        {
            ContainerDialogueEventSO containerDialogueEventSO = new();

            if (existContainerDialogueEvent != null)
            {
                containerDialogueEventSO.DialogueEventSO = existContainerDialogueEvent.DialogueEventSO;
            }
            EventData.ContainerDialogueEventSOs.Add(containerDialogueEventSO);

            Box boxContainer = new();
            boxContainer.AddToClassList("EventBox");

            ObjectField objectField = GetNewObjectFieldDialogueEventSO(containerDialogueEventSO);

            Button removeButton = GetNewButton("X", "RemoveButton");
            removeButton.clicked += () =>
            {
                DeleteBox(boxContainer);
                EventData.ContainerDialogueEventSOs.Remove(containerDialogueEventSO);
            };
            
            boxContainer.Add(objectField);
            boxContainer.Add(removeButton);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }
    }
}