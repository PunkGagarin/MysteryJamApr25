using Jam.Scripts.Quests.Data;
using UnityEngine;

namespace Jam.Scripts.Quests
{
    public class QuestModel
    {
        private Quest _activeQuest;

        public void AddQuest(Quest quest) =>
            _activeQuest = quest;

        public bool HaveQuest(int questId) =>
            _activeQuest != null && _activeQuest.Id == questId;

        public bool TryRemoveQuest()
        {
            if (_activeQuest != null)
            {
                _activeQuest = null;
                return true;
            }

            Debug.LogError($"No active quest!");
            return false;
        }

        public void SetComplete()
        {
            if (_activeQuest != null)
                _activeQuest.IsComplete = true;
            else
                Debug.LogError($"No active quest!");
        }
        
        public void SetIncomplete()
        {
            if (_activeQuest != null)
                _activeQuest.IsComplete = false;
            else
                Debug.LogError($"No active quest!");
        }

        
        public void SetFail()
        {
            if (_activeQuest != null)
                _activeQuest.IsFailed = true;
            else
                Debug.LogError($"No active quest!");
        }
        
        public bool IsComplete()
        {
            if (_activeQuest != null)
                return _activeQuest.IsComplete;

            Debug.LogError($"No active quest!");
            return false;
        }
        
        public bool IsFailed()
        {
            if (_activeQuest != null)
                return _activeQuest.IsFailed;

            Debug.LogError($"No active quest!");
            return false;
        }

        public bool HaveAnyQuest() => 
            _activeQuest != null;
    }
}
