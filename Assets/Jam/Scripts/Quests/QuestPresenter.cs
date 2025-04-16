using System;
using Jam.Scripts.Quests.Data;
using Jam.Scripts.Ritual.Components;
using Zenject;

namespace Jam.Scripts.Quests
{
    public class QuestPresenter
    {
        [Inject] private QuestRepository _questsRepository;
        private QuestModel _questModel;
       
        public event Action<Quest> OnQuestAdded;

        public QuestPresenter()
        {
            _questModel = new QuestModel();
        }

        public void AddQuest(int questId)
        {
            QuestDefinition questDefinition = _questsRepository.GetQuest(questId);
            Quest quest = new Quest(questDefinition);
            _questModel.AddQuest(quest);
            OnQuestAdded?.Invoke(quest);
        }
        
        public void RemoveQuest() => 
            _questModel.TryRemoveQuest();

        public void SetComplete() => 
            _questModel.SetComplete();
        
        public void SetFail() => 
            _questModel.SetFail();
        
        public void SetIncomplete() => 
            _questModel.SetIncomplete();
        
        public bool HaveQuest(int questId) =>
            _questModel.HaveQuest(questId);
        
        public bool IsQuestComplete() => 
            _questModel.IsComplete();
        
        public bool IsQuestFailed() => 
            _questModel.IsFailed();

        public bool HaveAnyQuest() => 
            _questModel.HaveAnyQuest();
    }
}
