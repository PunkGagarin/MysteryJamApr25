using System;
using System.Collections.Generic;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.Quests;
using Jam.Scripts.Ritual;
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
        [SerializeField, StringEvent] private string _componentEvent;
        [SerializeField, StringEvent] private string _attemptsEvent;

        [Inject] private PlayerStatsPresenter _playerStats;
        [Inject] private QuestPresenter _questPresenter;
        [Inject] private RitualController _ritualController;
 
        private Dictionary<string, Action<float, StringEventModifierType>> _eventHandlers;
        private Dictionary<string, StringEventCondition> _conditionHandlers;

        private void Start()
        {
            _eventHandlers = new Dictionary<string, Action<float, StringEventModifierType>>
            {
                { _questEvent, HandleQuest },
                { _componentEvent, HandleComponent },
                { _reputationEvent, HandleReputation },
                { _moneyEvent, HandleMoney },
            };
            
            _conditionHandlers = new Dictionary<string, StringEventCondition>
            {
                { _questEvent, CheckQuest },
                { _attemptsEvent, CheckAttempts },
                { _moneyEvent, CheckMoney}
            };
        }

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

        public void DialogueModifierEvents(string stringEvent, StringEventModifierType modifierType, float value = 0)
        {
            if (_eventHandlers.TryGetValue(stringEvent, out var handler))
            {
                handler.Invoke(value, modifierType);
            }
            else
            {
                Debug.LogWarning($"Неизвестное событие: {stringEvent}");
            }
        }

        public bool DialogueConditionEvents(string stringEvent, StringEventConditionType conditionType, float value = 0)
        {
            if (_conditionHandlers.TryGetValue(stringEvent, out var handler))
            {
                return handler.Invoke(value, conditionType);
            }
            
            Debug.LogWarning($"Неизвестное событие: {stringEvent}");
            return false;
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
            switch (modifierType)
            {
                case StringEventModifierType.SetTrue:
                    _questPresenter.SetComplete();
                    break;
                case StringEventModifierType.SetFalse:
                    _questPresenter.SetIncomplete();
                    break;
                case StringEventModifierType.Add:
                    _questPresenter.AddQuest((int)value);
                    break;
                case StringEventModifierType.Remove:
                    _questPresenter.RemoveQuest();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifierType), modifierType, null);
            }
        }
        
        private void HandleComponent(float id, StringEventModifierType modifierType)
        {
            switch (modifierType)
            {
                case StringEventModifierType.Add:
                    
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
            if (!_questPresenter.HaveQuest((int)questId))
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
    }
}
