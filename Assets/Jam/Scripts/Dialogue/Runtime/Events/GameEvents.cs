using System;
using System.Collections.Generic;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.Quests;
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

        [Inject] private PlayerStats _playerStats;
        [Inject] private QuestPresenter _questPresenter;
        
        private Dictionary<string, Action<float, StringEventModifierType>> _eventHandlers;
        private Dictionary<string, StringEventCondition> _conditionHandlers;
        
        private void Start()
        {
            _eventHandlers = new Dictionary<string, Action<float, StringEventModifierType>>
            {
                { _questEvent, HandleQuest },
            };
            
            _conditionHandlers = new Dictionary<string, StringEventCondition>
            {
                { _questEvent, CheckQuest },
            };
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
        }

        private void HandleQuest(float value, StringEventModifierType modifierType)
        {
            switch (modifierType)
            {
                case StringEventModifierType.SetTrue:
                    _questPresenter.SetComplete((int)value);
                    break;
                case StringEventModifierType.SetFalse:
                    _questPresenter.SetIncomplete((int)value);
                    break;
                case StringEventModifierType.Add:
                    _questPresenter.AddQuest((int)value);
                    break;
                case StringEventModifierType.Remove:
                    _questPresenter.RemoveQuest((int)value);
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
                    return _questPresenter.IsQuestComplete((int)questId);
                case StringEventConditionType.False:
                    return _questPresenter.IsQuestFailed((int)questId);
                case StringEventConditionType.Equals:
                    return true;
            }

            Debug.LogError($"Wrong condition type for quests: {conditionType}!");
            return false;
        }
    }
}
