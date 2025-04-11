using System;
using Jam.Scripts.Quests.Data;
using Zenject;

namespace Jam.Scripts.Quests
{
    public class QuestPresenter
    {
        [Inject] private QuestRepository _questsRepository;
        private QuestModel _questModel;
       
        public event Action<int> OnQuestRemoved;

        public QuestPresenter()
        {
            _questModel = new QuestModel();
        }

        public void AddQuest(int questId)
        {
            QuestDefinition questDefinition = _questsRepository.GetQuest(questId);
            Quest quest = new Quest(questDefinition);
            _questModel.AddQuest(quest);
        }
        
        public void RemoveQuest(int questId)
        {
            if (_questModel.TryRemoveQuest(questId))
                OnQuestRemoved?.Invoke(questId);
        }
        
        public void SetComplete(int questId) => 
            _questModel.SetComplete(questId);
        
        public void SetIncomplete(int questId) => 
            _questModel.SetIncomplete(questId);
        
        public bool HaveQuest(int questId) =>
            _questModel.HaveQuest(questId);
        
        public bool IsQuestComplete(int questId) => 
            _questModel.IsComplete(questId);
    }
}
