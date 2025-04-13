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
            Debug.Log($"set ritual {_currentRitual.Name}");
        }
        
        private void StartRitual()
        {
            _attempt++;

            bool areEqual = AreComponentListsEqual(_currentRitual.Components, _components.Select(component => component.ComponentInside).ToList());
            
            if (areEqual)
            {
                Debug.Log($"Ritual OK");
                _questPresenter.SetComplete(_currentQuestId);
            }
            else
            {
                Debug.Log($"Ritual failed");
                if (_attempt >= _attemptsBeforeFail)
                {
                    Debug.Log("Quest failed");
                    _questPresenter.SetFail(_currentQuestId);
                }
            }
            
            
            ClearTable();
            UpdateButtons();
        }
        
        private bool AreComponentListsEqual(List<ComponentDefinition> first, List<ComponentDefinition> second)
        {
            var firstGrouped = first.GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());

            var secondGrouped = second.GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());

            return firstGrouped.Count == secondGrouped.Count &&
                firstGrouped.All(kv => secondGrouped.TryGetValue(kv.Key, out var count) && count == kv.Value);
        }
        
        private void UpdateButtons()
        {
            bool isActive = _components.Any(component => !component.IsFree);
            _clearTable.gameObject.SetActive(isActive);
            _startRitual.gameObject.SetActive(isActive);
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
