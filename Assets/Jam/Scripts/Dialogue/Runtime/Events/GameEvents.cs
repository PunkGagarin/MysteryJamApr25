using System;
using System.Collections.Generic;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.Dialogue.Utils;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.Utils.String_Tool;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Dialogue.Runtime.Events
{
    public class GameEvents : MonoBehaviour
    {
        [SerializeField, StringEvent] private string _reputationEvent;
        [SerializeField, StringEvent] private string _questEvent;

        [Inject] private PlayerStats _playerStats;
        
        private Dictionary<string, Action<float, StringEventModifierType>> _eventHandlers;
        
        private void Start()
        {
            _eventHandlers = new Dictionary<string, Action<float, StringEventModifierType>>
            {
                { _reputationEvent, HandleReputation },
                { _questEvent, HandleQuest },
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
            if (stringEvent.Equals(_reputationEvent))
                return UseStringEventCondition.ConditionFloatCheck(_playerStats.Reputation, value, conditionType);
            
            return false;
        }
        
        private void HandleReputation(float value, StringEventModifierType modifierType)
        {
            Debug.Log($"Player reputation modified by {value} with modifier {modifierType}");
        }

        private void HandleQuest(float value, StringEventModifierType modifierType)
        {
            Debug.Log($"Player got quest with ID {value}, type {modifierType}");
        }
    }
}
