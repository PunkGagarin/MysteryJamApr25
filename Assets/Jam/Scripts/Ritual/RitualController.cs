using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Quests;
using Jam.Scripts.Quests.Data;
using Jam.Scripts.Ritual.Components;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Ritual
{
    public class RitualController : MonoBehaviour
    {
        [SerializeField] private Button _startRitual;
        [SerializeField] private Button _clearTable;
        [SerializeField] private List<ComponentRoom> _components;
        [SerializeField] private int _attemptsBeforeFail;

        [Inject] private QuestPresenter _questPresenter;
        [Inject] private QuestRepository _questRepository;

        private RitualDefinition _currentRitual;
        private int _attempt;
        private int _currentQuestId;

        public bool TryAddComponent(ComponentDefinition componentToAdd, out ComponentRoom componentRoom)
        {
            componentRoom = null;
            if (_components.All(componentRoom => !componentRoom.IsFree))
                return false;

            if (_currentRitual == null)
                return false;
            
            componentRoom = _components.First(componentRoom => componentRoom.IsFree);  
            
            UpdateButtons();

            componentRoom.SetComponent(componentToAdd);
            return true;
        }

        private void ClearTable()
        {
            foreach (ComponentRoom componentRoom in _components)
                componentRoom.ReleaseComponent();

            UpdateButtons();
        }
        
        private void SetRitual(int questId)
        {
            _currentQuestId = questId;
            _currentRitual = _questRepository.GetQuest(questId).Ritual;
        }
        
        private void StartRitual()
        {
            _attempt++;

            bool areEqual = _currentRitual.Components.All(_components.Select(component => component.ComponentInside).Contains);

            if (areEqual)
                _questPresenter.SetComplete(_currentQuestId);
            
            else if (_attempt >= _attemptsBeforeFail)
                _questPresenter.SetFail(_currentQuestId);
            
            UpdateButtons();
        }
        
        private void UpdateButtons()
        {
            _clearTable.gameObject.SetActive(_components.Any(component => !component.IsFree));
            _startRitual.gameObject.SetActive(_components.All(componentRoom => !componentRoom.IsFree));
        }
        
        private void Awake()
        {
            UpdateButtons();
            
            _clearTable.onClick.AddListener(ClearTable);
            _startRitual.onClick.AddListener(StartRitual);
            _questPresenter.OnQuestAdded += SetRitual;
        }
        
        private void OnDestroy()
        {
            _clearTable.onClick.RemoveListener(ClearTable);
            _startRitual.onClick.RemoveListener(StartRitual);
            _questPresenter.OnQuestAdded -= SetRitual;
        }
    }
}
