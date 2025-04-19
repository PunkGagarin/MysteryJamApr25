using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.Dialogue.Runtime.Events;
using Jam.Scripts.Dialogue.Runtime.SO;
using Jam.Scripts.Dialogue.Runtime.SO.Dialogue;
using Jam.Scripts.Dialogue.UI;
using Jam.Scripts.Ritual;
using Jam.Scripts.Utils.String_Tool;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Jam.Scripts.Dialogue.Gameplay
{
    public class DialogueRunner : MonoBehaviour
    {
        [SerializeField, StringEvent] private string _ritualEvent;
        [SerializeField] private DialogueView _dialogueView;
        [Inject] private LanguageController _languageController;
        [Inject] private GameEvents _gameEvents;
        [Inject] private RitualController _ritualController;

        private DialogueContainerSO _dialogueContainer;
        
        private DialogueData _currentDialogueNodeData;
        private DialogueData _lastDialogueNodeData;

        private List<DialogueDataBaseContainer> _dialogueNodes;
        private int _currentIndex = 0;
        private bool _isDialogueActive = false;
        private bool _isGhostTalking;
        private BranchData _waitingBranchData;
        private Action _leaveEvent;
        
        public bool IsBranchWaiting => _waitingBranchData != null;

        public void StartDialogue(DialogueContainerSO dialogueContainer, Action closeEvent = null)
        {
            _isDialogueActive = true;
            _dialogueContainer = dialogueContainer;

            _leaveEvent = closeEvent;
            _dialogueView.Open();
            
            CheckNodeType(GetNextNode(_dialogueContainer.StartData[0]));
        }
        
        private BaseData GetNodeByGuid(string targetNodeGuid) =>
            _dialogueContainer.AllData.Find(node => node.NodeGuid == targetNodeGuid); 

        private BaseData GetNodeByNodePort(DialogueDataPort nodePort) =>
            _dialogueContainer.AllData.Find(node => node.NodeGuid == nodePort.InputGuid);

        private BaseData GetNextNode(BaseData baseNodeData)
        {
            LinkData nodeLinkData = _dialogueContainer.NodeLinkData.Find(edge => edge.BaseNodeGuid == baseNodeData.NodeGuid);
            return GetNodeByGuid(nodeLinkData.TargetNodeGuid);
        }
        
        private void CheckNodeType(BaseData baseNodeData)
        {
            switch (baseNodeData)
            {
                case StartData:
                    RunStartNode();
                    break;
                case DialogueData dialogueData:
                    RunDialogueNode(dialogueData);
                    break;
                case EventData eventData:
                    RunEventNode(eventData);
                    break;
                case BranchData branchData:
                    RunBranchNode(branchData);
                    break;
                case EndData endData:
                    RunEndNode(endData);
                    break;
            }
        }

        private void RunStartNode()
        {
            CheckNodeType(GetNextNode(_dialogueContainer.StartData[0]));
        }

        private void RunDialogueNode(DialogueData nodeData)
        {
            _currentDialogueNodeData = nodeData;

            _dialogueNodes = new List<DialogueDataBaseContainer>();
            _dialogueNodes.AddRange(nodeData.DialogueDataImages);
            _dialogueNodes.AddRange(nodeData.DialogueDataNames);
            _dialogueNodes.AddRange(nodeData.DialogueDataTexts);

            _currentIndex = 0;
            
            _dialogueNodes.Sort((x,y) => x.ID.Value.CompareTo(y.ID.Value));

            DoTheDialogue();
        }

        private void DoTheDialogue()
        {
            _dialogueView.HideButtons();

            for (int i = _currentIndex; i < _dialogueNodes.Count; i++)
            {
                _currentIndex = i + 1;
                if (_dialogueNodes[i] is DialogueDataName dataName)
                {
                    if (string.Equals(dataName.CharacterName
                            .Find(text => 
                                text.LanguageType == _languageController.CurrentLanguage).LanguageGenericType, "Ghost", StringComparison.InvariantCultureIgnoreCase))
                        
                        _isGhostTalking = true;
                }
                else if (_dialogueNodes[i] is DialogueDataText dataText)
                {
                    _dialogueView.SetText(dataText.Text.Find(text => text.LanguageType == _languageController.CurrentLanguage).LanguageGenericType, _isGhostTalking, ShowButtons);
                    if (_isGhostTalking)
                        _isGhostTalking = false;
                    break;
                }
            }
        }

        private void ShowButtons()
        {
            List<DialogueButtonContainer> dialogueButtonContainers = new();
            if (_currentIndex == _dialogueNodes.Count)
            {
                if (_currentDialogueNodeData.DialogueDataPorts.Count == 0)
                {
                    void OnUnityAction() => CheckNodeType(GetNextNode(_currentDialogueNodeData));
                    dialogueButtonContainers.Add(GetContinueButton(OnUnityAction));
                    _dialogueView.SetButtons(dialogueButtonContainers);
                }
                else
                {
                    _currentDialogueNodeData.DialogueDataPorts.ForEach(port => ChoiceCheck(port.InputGuid, dialogueButtonContainers));
                    _dialogueView.SetButtons(dialogueButtonContainers);
                }
            }
            else
            {
                dialogueButtonContainers.Add(GetContinueButton(DoTheDialogue));
                _dialogueView.SetButtons(dialogueButtonContainers);
            }
        }

        private DialogueButtonContainer GetContinueButton(UnityAction unityAction)
        {
            DialogueButtonContainer buttonContainer = new DialogueButtonContainer()
            {
                Text = "Continue",
                ConditionCheck = true,
                UnityAction = unityAction,
                ChoiceStateType = default
            };
            return buttonContainer;
        }

        private void ChoiceCheck(string guidID, List<DialogueButtonContainer> dialogueButtonContainers)
        {
            ChoiceData choiceNode = GetNodeByGuid(guidID) as ChoiceData;
            DialogueButtonContainer dialogueButtonContainer = new();

            bool checkBranch = choiceNode != null && choiceNode.EventDataStringConditions.All(item => _gameEvents.DialogueConditionEvents(item.StringEventText.Value, item.StringEventConditionType.Value, item.Number.Value));

            void OnUnityAction() => CheckNodeType(GetNextNode(choiceNode));

            dialogueButtonContainer.ChoiceStateType = choiceNode.ChoiceStateType.Value;
            dialogueButtonContainer.Text = choiceNode.Text.Find(text => text.LanguageType == _languageController.CurrentLanguage).LanguageGenericType;
            dialogueButtonContainer.UnityAction = OnUnityAction;
            dialogueButtonContainer.ConditionCheck = checkBranch;

            dialogueButtonContainers.Add(dialogueButtonContainer);
        }
        
        private void RunEventNode(EventData nodeData)
        {
            if (!_isDialogueActive)
                return;
            
            nodeData.ContainerDialogueEventSOs.ForEach(item => item.DialogueEventSO.RunEvent());
            nodeData.EventDataStringModifiers.ForEach(item => _gameEvents.DialogueModifierEvents(item.StringEventText.Value, item.StringEventModifierType.Value, item.Number.Value));
            
            CheckNodeType(GetNextNode(nodeData));
        }
        
        private void RunBranchNode(BranchData nodeData)
        {
            if (nodeData.EventDataStringConditions.Any(item => item.StringEventText.Value == _ritualEvent))
            {
                _waitingBranchData = nodeData;
                return;
            }
            bool checkBranch = nodeData.EventDataStringConditions.All(item => _gameEvents.DialogueConditionEvents(item.StringEventText.Value, item.StringEventConditionType.Value, item.Number.Value));

            string nextNode = checkBranch ? nodeData.TrueGuidNode : nodeData.FalseGuidNode;
            CheckNodeType(GetNodeByGuid(nextNode));
        }

        private void RunEndNode(EndData nodeData)
        {
            switch (nodeData.EndNodeType.Value)
            {
                case EndNodeType.End:
                    _isDialogueActive = false;
                    _dialogueView.Close();
                    break;
                case EndNodeType.Repeat:
                    CheckNodeType(GetNodeByGuid(_currentDialogueNodeData.NodeGuid));
                    break;
                case EndNodeType.ReturnToStart:
                    CheckNodeType(GetNextNode(_dialogueContainer.StartData[0]));
                    break;
                case EndNodeType.EndAndLeave:
                    _isDialogueActive = false;
                    _dialogueView.Close();
                    _leaveEvent?.Invoke();
                    break;
            }
        }

        private void ReactOnRitual()
        {
            if (!IsBranchWaiting)
                return;
            
            RunWaitingBranchNode();
        }

        private void RunWaitingBranchNode()
        {
            bool checkBranch = _waitingBranchData.EventDataStringConditions.All(item => _gameEvents.DialogueConditionEvents(item.StringEventText.Value, item.StringEventConditionType.Value, item.Number.Value));

            string nextNode = checkBranch ? _waitingBranchData.TrueGuidNode : _waitingBranchData.FalseGuidNode;
            _waitingBranchData = null;
            CheckNodeType(GetNodeByGuid(nextNode));
        }

        private void Awake()
        {
            _ritualController.OnRitual += ReactOnRitual;
        }

        private void OnDestroy()
        {
            _ritualController.OnRitual -= ReactOnRitual;
        }
    }
}