using System;
using System.Collections.Generic;
using System.Globalization;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.Dialogue.Runtime.Events.SO;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.Quests;
using Jam.Scripts.Ritual;
using Jam.Scripts.Ritual.Inventory;
using Jam.Scripts.Ritual.Inventory.Reagents;
using Jam.Scripts.Ritual.Tools;
using Jam.Scripts.Tutorial;
using Jam.Scripts.Utils.String_Tool;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Dialogue.Runtime.Events
{
    public class GameEvents : MonoBehaviour
    {
        delegate bool StringEventCondition(float value, StringEventConditionType conditionType);
        
        [SerializeField, StringEvent] private string _questEvent;
        [SerializeField, StringEvent] private string _reputationEvent;
        [SerializeField, StringEvent] private string _moneyEvent;
        [SerializeField, StringEvent] private string _reagentEvent;
        [SerializeField, StringEvent] private string _attemptsEvent;
        [SerializeField, StringEvent] private string _ritualEvent;
        [SerializeField, StringEvent] private string _ritualFailedByAgeEvent;
        [SerializeField, StringEvent] private string _ritualFailedBySexEvent;
        [SerializeField, StringEvent] private string _ritualFailedByRaceEvent;
        [SerializeField, StringEvent] private string _ritualFailedByDeathEvent;
        [SerializeField, StringEvent] private string _ritualFailedByExcludedEvent;
        [SerializeField, StringEvent] private string _addToolEvent;
        [SerializeField, StringEvent] private string _tutorialStepEvent;

        [Inject] private PlayerStatsPresenter _playerStats;
        [Inject] private QuestPresenter _questPresenter;
        [Inject] private RitualController _ritualController;
        [Inject] private InventorySystem _inventorySystem;
        [Inject] private InventoryConfig _inventoryConfig;
        [Inject] private ToolController _toolController;
        [Inject] private TutorialService _tutorialService;
 
        private Dictionary<string, Action<float, StringEventModifierType>> _eventHandlers;
        private Dictionary<string, StringEventCondition> _conditionHandlers;

        public void DialogueModifierEvents(string stringEvent, StringEventModifierType modifierType, float value = 0)
        {
            if (_eventHandlers.TryGetValue(stringEvent, out var handler))
                handler.Invoke(value, modifierType);
            else
                Debug.LogWarning($"Неизвестное событие: {stringEvent}");
        }

        public bool DialogueConditionEvents(string stringEvent, StringEventConditionType conditionType, float value = 0)
        {
            if (_conditionHandlers.TryGetValue(stringEvent, out var handler))
                return handler.Invoke(value, conditionType);
            
            Debug.LogWarning($"Неизвестное событие: {stringEvent}");
            return false;
        }
        
        private void Start()
        {
            _eventHandlers = new Dictionary<string, Action<float, StringEventModifierType>>
            {
                { _questEvent, HandleQuest },
                { _reagentEvent, HandleReagent },
                { _reputationEvent, HandleReputation },
                { _moneyEvent, HandleMoney },
                { _addToolEvent, AddTool },
                { _tutorialStepEvent, HandleTutorialEvent },
            };

            _conditionHandlers = new Dictionary<string, StringEventCondition>
            {
                { _questEvent, CheckQuest },
                { _attemptsEvent, CheckAttempts },
                { _moneyEvent, CheckMoney },
                { _ritualEvent, CheckRitual },
                { _ritualFailedByAgeEvent, CheckRitualFailedByAge },
                { _ritualFailedBySexEvent, CheckRitualFailedBySex },
                { _ritualFailedByRaceEvent, CheckRitualFailedByRace },
                { _ritualFailedByDeathEvent, CheckRitualFailedByDeath },
                { _ritualFailedByExcludedEvent, CheckRitualFailedByExcludedReagent },
                { _tutorialStepEvent, CheckTutorial },
            };
        }

        private bool CheckTutorial(float value, StringEventConditionType conditionType)
        {
            int checkValue = (int)value;
            switch (conditionType)
            {
                case StringEventConditionType.Equals:
                    return _tutorialService.TutorialStep == checkValue;
                case StringEventConditionType.EqualsOrBigger:
                    return _tutorialService.TutorialStep >= checkValue;
                case StringEventConditionType.EqualsOrSmaller:
                    return _tutorialService.TutorialStep <= checkValue;
                case StringEventConditionType.Bigger:
                    return _tutorialService.TutorialStep > checkValue;
                case StringEventConditionType.Smaller:
                    return _tutorialService.TutorialStep < checkValue;
                case StringEventConditionType.True:
                case StringEventConditionType.False:
                default:
                    return false;
            }
        }

        private void HandleTutorialEvent(float value, StringEventModifierType modifierType)
        {
            int intValue = (int)value;
            switch (modifierType)
            {
                case StringEventModifierType.Add:
                    _tutorialService.TutorialStep += intValue;
                    break;
                case StringEventModifierType.Remove:
                    _tutorialService.TutorialStep -= intValue;
                    break;
                case StringEventModifierType.SetFalse:
                case StringEventModifierType.SetTrue:
                    _tutorialService.TutorialEvent(intValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifierType), modifierType, null);
            }
        }

        private bool CheckRitualFailedByExcludedReagent(float value, StringEventConditionType conditionType)
        {
            if (value == 0)
                return _ritualController.RitualFailedByExcludedReagents;

            var matchReagent = value.ToString(CultureInfo.InvariantCulture).Split(".");
            int excludedReagentId1 = int.Parse(matchReagent[0]);
            int excludedReagentId2 = int.Parse(matchReagent[1]);
            
            foreach (var excludedReagent in _ritualController.ExcludedReagents)
            {
                if ((excludedReagent.ReagentId == excludedReagentId1 || excludedReagent.ExcludedReagentId == excludedReagentId1) 
                    && (excludedReagent.ReagentId == excludedReagentId2 || excludedReagent.ExcludedReagentId == excludedReagentId2))
                    return true;
            }

            return false;
        }
        
        private bool CheckRitualFailedByDeath(float value, StringEventConditionType conditionType) => 
            _ritualController.RitualFailedByMissingDeathReagent;
        
        private bool CheckRitualFailedByRace(float value, StringEventConditionType conditionType) => 
            _ritualController.RitualFailedByMissingRaceReagent;
        
        private bool CheckRitualFailedBySex(float value, StringEventConditionType conditionType) => 
            _ritualController.RitualFailedByMissingSexReagent;
        
        private bool CheckRitualFailedByAge(float value, StringEventConditionType conditionType) => 
            _ritualController.RitualFailedByMissingAgeReagent;

        private bool CheckRitual(float _, StringEventConditionType __) => 
            _questPresenter.IsQuestComplete();

        private bool CheckMoney(float value, StringEventConditionType conditionType) => 
            _playerStats.CheckMoney((int)value, conditionType);

        private bool CheckAttempts(float value, StringEventConditionType conditionType)
        {
            switch (conditionType)
            {
                case StringEventConditionType.Equals:
                    return _ritualController.Attempt == (int)value;
                case StringEventConditionType.EqualsOrBigger:
                    return _ritualController.Attempt >= (int)value;
                case StringEventConditionType.EqualsOrSmaller:
                    return _ritualController.Attempt <= (int)value;
                case StringEventConditionType.Bigger:
                    return _ritualController.Attempt > (int)value;
                case StringEventConditionType.Smaller:
                    return _ritualController.Attempt < (int)value;
                case StringEventConditionType.True:
                case StringEventConditionType.False:
                default:
                    return false;
            }
        }
        
        private void HandleReputation(float value, StringEventModifierType modifierType)
        {
            Debug.Log($"Player reputation modified by {value} with modifier {modifierType}");
            switch (modifierType)
            {
                case StringEventModifierType.Add:
                    _playerStats.AddReputation((int)value);
                    break;
                case StringEventModifierType.Remove:
                    _playerStats.RemoveReputation((int)value);
                    break;   
                case StringEventModifierType.SetTrue:
                case StringEventModifierType.SetFalse:
                default:
                    Debug.Log("Wrong modifier type");
                    break;
            }
        }
        
        private void HandleMoney(float value, StringEventModifierType modifierType)
        {
            Debug.Log($"Player money modified by {value} with modifier {modifierType}");
            switch (modifierType)
            {
                case StringEventModifierType.Add:
                    _playerStats.AddMoney((int)value);
                    break;
                case StringEventModifierType.Remove:
                    _playerStats.RemoveMoney((int)value);
                    break;   
                case StringEventModifierType.SetTrue:
                case StringEventModifierType.SetFalse:
                default:
                    Debug.Log("Wrong modifier type");
                    break;
            }
        }

        private void HandleQuest(float value, StringEventModifierType modifierType)
        {
            //Debug.Log($"Player quest modified by {value} with modifier {modifierType}");
            switch (modifierType)
            {
                case StringEventModifierType.SetTrue:
                    _questPresenter.SetComplete();
                    break;
                case StringEventModifierType.SetFalse:
                    _questPresenter.SetIncomplete();
                    break;
                case StringEventModifierType.Add:
                    _questPresenter.AddQuest();
                    break;
                case StringEventModifierType.Remove:
                    _questPresenter.RemoveQuest();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifierType), modifierType, null);
            }
        }
        
        private void HandleReagent(float id, StringEventModifierType modifierType)
        {
            switch (modifierType)
            {
                case StringEventModifierType.Add:
                    _inventorySystem.AddReagent((int)id, _inventoryConfig.MaxReagentAmount);
                    break;
                case StringEventModifierType.Remove:
                    break;
                case StringEventModifierType.SetTrue:
                case StringEventModifierType.SetFalse:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifierType), modifierType, null);
            }
        }
        
        private bool CheckQuest(float questId, StringEventConditionType conditionType)
        {
            if (!_questPresenter.HaveQuest())
                return false;
            
            switch (conditionType)
            {
                case StringEventConditionType.True:
                    return _questPresenter.IsQuestComplete();
                case StringEventConditionType.False:
                    return _questPresenter.IsQuestFailed();
                case StringEventConditionType.Equals:
                    return true;
            }

            Debug.LogError($"Wrong condition type for quests: {conditionType}!");
            return false;
        }

        private void AddTool(float toolId, StringEventModifierType eventModifier)
        {
            if (eventModifier == StringEventModifierType.Add) 
                _toolController.UnlockTool((int)toolId);
        }

        public void RunEvent(DialogueEventSO itemDialogueEventSo)
        {
            if (itemDialogueEventSo is AddReagentsEvent addReagentsEvent)
            {
                addReagentsEvent.OnReagentsAdded += AddReagents;
                addReagentsEvent.RunEvent();
                addReagentsEvent.OnReagentsAdded -= AddReagents;

                void AddReagents(List<ReagentDefinition> reagentDefinitions)
                {
                    foreach (var reagentDefinition in reagentDefinitions) 
                        _inventorySystem.AddReagent(reagentDefinition.Id, _inventoryConfig.MaxReagentAmount);
                }
            }

            if (itemDialogueEventSo is RewardEvent rewardEvent)
            {
                rewardEvent.OnReward += AddRewards;
                rewardEvent.RunEvent();
                rewardEvent.OnReward -= AddRewards;

                void AddRewards(Reward reward)
                {
                    _playerStats.AddMoney(reward.MoneyReward);
                    _playerStats.AddReputation(reward.ReputationReward);

                    if (reward.ReagentsReward == null) 
                        return;
                    
                    foreach (var reagent in reward.ReagentsReward)
                        _inventorySystem.AddReagent(reagent.Id, _inventoryConfig.MaxReagentAmount);
                }
            }

            if (itemDialogueEventSo is PenaltyEvent penaltyEvent)
            {
                penaltyEvent.OnPenalty += AddPenalty;
                penaltyEvent.RunEvent();
                penaltyEvent.OnPenalty -= AddPenalty;

                void AddPenalty(PenaltyEvent.Penalty reward)
                {
                    _playerStats.RemoveMoney(reward.MoneyPenalty);
                    _playerStats.RemoveReputation(reward.ReputationPenalty);
                }
            }
        }
    }
}
