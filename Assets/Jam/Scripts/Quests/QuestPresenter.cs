using System;
using Jam.Scripts.Quests.Data;
using Jam.Scripts.VFX;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Quests
{
    public class QuestPresenter
    {
        [Inject] private QuestRepository _questsRepository;
        [Inject] private PointerFirefly _pointerFirefly;
        private QuestModel _questModel;
        private int _currentCharacterId;

        public event Action<Quest> OnQuestAdded;

        public QuestPresenter()
        {
            _questModel = new QuestModel();
        }

        public void AddQuest()
        {
            QuestDefinition questDefinition = _questsRepository.GetQuest(_currentCharacterId);
            Quest quest = new Quest(questDefinition);
            _questModel.AddQuest(quest);
            Debug.Log($"added quest {quest.Id}");
            if (!_questModel.HaveAnyCompletedQuest())
                _pointerFirefly.ChangeTargetTo(TargetType.Book);
            OnQuestAdded?.Invoke(quest);
        }

        public void SetCharacter(int id) =>
            _currentCharacterId = id;

        public void RemoveQuest()
        {
            _questModel.TryRemoveQuest();
            _pointerFirefly.ChangeTargetTo(
                _questModel.HaveAnyCompletedQuest() 
                    ? TargetType.Book
                    : TargetType.Rope
                );
        }

        public void SetComplete() =>
            _questModel.SetComplete();

        public void SetFail() =>
            _questModel.SetFail();

        public void SetIncomplete() =>
            _questModel.SetIncomplete();

        public bool HaveQuest() =>
            _questModel.HaveQuest(_currentCharacterId);

        public bool IsQuestComplete() =>
            _questModel.IsComplete();

        public bool IsQuestFailed() =>
            _questModel.IsFailed();

        public bool HaveAnyQuest() =>
            _questModel.HaveAnyQuest();

        public bool HaveAnyCompletedQuest() =>
            _questModel.HaveAnyCompletedQuest();
    }
}