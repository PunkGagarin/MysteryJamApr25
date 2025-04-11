using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Quests.Data;
using UnityEngine;

namespace Jam.Scripts.Quests
{
    public class QuestModel
    {
        public List<Quest> ActiveQuests { get; private set; }
        
       
        public QuestModel()
        {
            ActiveQuests = new List<Quest>();
        }

        public void AddQuest(Quest questDefinition)
        {
            ActiveQuests.Add(questDefinition);
        }
        
        public bool HaveQuest(int questId) => 
            ActiveQuests.Any(quest => quest.Id == questId);

        public bool TryRemoveQuest(int questId)
        {
            Quest quest = GetQuest(questId);
            
            if (quest != null)
            {
                ActiveQuests.Remove(quest);
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
            foreach (Quest activeQuest in ActiveQuests)
            {
                if (activeQuest.Id == questId)
                    return activeQuest;
            }
            return null;
        }
    }
}
