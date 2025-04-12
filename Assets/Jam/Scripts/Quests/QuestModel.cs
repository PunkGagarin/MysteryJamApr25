using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Quests.Data;
using UnityEngine;

namespace Jam.Scripts.Quests
{
    public class QuestModel
    {
        private List<Quest> _activeQuests;
       
        public QuestModel()
        {
            _activeQuests = new List<Quest>();
        }

        public void AddQuest(Quest questDefinition) => 
            _activeQuests.Add(questDefinition);

        public bool HaveQuest(int questId) => 
            _activeQuests.Any(quest => quest.Id == questId);

        public bool TryRemoveQuest(int questId)
        {
            Quest quest = GetQuest(questId);
            
            if (quest != null)
            {
                _activeQuests.Remove(quest);
                return true;
            }

            Debug.LogError($"No active quest with id {questId}!");
            return false;
        }

        public void SetComplete(int questId)
        {
            Quest quest = GetQuest(questId);
            
            if (quest != null)
            {
                quest.IsComplete = true;
                return;
            }
            
            Debug.LogError($"No active quest with id {questId}!");
        }
        
        public bool IsComplete(int questId)
        {
            Quest quest = GetQuest(questId);
            
            if (quest != null)
                return quest.IsComplete;
            
            Debug.LogError($"No active quest with id {questId}!");
            return false;
        }
        
        public void SetIncomplete(int questId)
        {
            Quest quest = GetQuest(questId);
            
            if (quest != null)
            {
                quest.IsComplete = false;
                return;
            }
            
            Debug.LogError($"No active quest with id {questId}!");
        }

        private Quest GetQuest(int questId)
        {
            foreach (Quest activeQuest in _activeQuests)
            {
                if (activeQuest.Id == questId)
                    return activeQuest;
            }
            return null;
        }
    }
}
