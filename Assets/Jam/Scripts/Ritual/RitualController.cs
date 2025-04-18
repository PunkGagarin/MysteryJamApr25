using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Audio.Domain;
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
        [Inject] private AudioService _audioService;

        private Quest _currentQuest;

        public int Attempt { get; private set; }

        public bool TryAddComponent(ComponentDefinition componentToAdd, out ComponentRoom componentRoom)
        {
            componentRoom = null;
            
            if (!_questPresenter.HaveAnyQuest() || _questPresenter.IsQuestComplete() || _questPresenter.IsQuestFailed())
                return false;
            
            if (_components.All(componentRoom => !componentRoom.IsFree))
                return false;
            
            componentRoom = _components.First(componentRoom => componentRoom.IsFree);  
            
            UpdateButtons();

            componentRoom.SetComponent(componentToAdd);
            return true;
        }

        private void SetQuest(Quest quest)
        {
            _currentQuest = quest;
            Attempt = 0;
        }

        private void ClearTable()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            foreach (ComponentRoom componentRoom in _components)
                componentRoom.ReleaseComponent();

            UpdateButtons();
        }
        
        private void StartRitual()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            Attempt++;

            List<ComponentDefinition> selectedComponents =
                _components.Where(component => component.ComponentInside != null).Select(component => component.ComponentInside).ToList();
            bool areComplete = CheckRitualState(selectedComponents);
            
            if (areComplete)
            {
                Debug.Log($"Ritual OK");
                _questPresenter.SetComplete();
            }
            else
            {
                Debug.Log($"Ritual failed");
                if (Attempt >= _attemptsBeforeFail)
                {
                    Debug.Log("Quest failed");
                    _questPresenter.SetFail();
                }
            }
            
            ClearTable();
            UpdateButtons();
        }
        
        private bool CheckRitualState(List<ComponentDefinition> selectedComponents)
        {
            if (!CheckForDeathReason(selectedComponents)) 
                return false;
            
            if (!CheckForAgeExcludes(selectedComponents)) 
                return false;

            if (!CheckForExcludedComponents(selectedComponents))
                return false;

            return CheckComponentMatches(selectedComponents);
        }

        private bool CheckComponentMatches(List<ComponentDefinition> selectedComponents) =>
            CheckForComponent(
                selectedComponents,
                _currentQuest.AgeType,
                c => c.AgeType,
                AgeType.None,
                "age")
            && CheckForComponent(
                selectedComponents,
                _currentQuest.SexType,
                c => c.SexType,
                SexType.None,
                "sex")
            && CheckForComponent(
                selectedComponents,
                _currentQuest.RaceType,
                c => c.RaceType,
                RaceType.None,
                "race")
            && CheckForComponent(
                selectedComponents,
                _currentQuest.DeathType,
                c => c.DeathType,
                DeathType.None,
                "death");

        private bool CheckForComponent<T>(
            List<ComponentDefinition> components,
            T currentQuestValue,
            Func<ComponentDefinition, T> selector,
            T noneValue,
            string typeName)
            where T : Enum
        {
            if (!currentQuestValue.Equals(noneValue))
            {
                if (components.All(c => !selector(c).Equals(currentQuestValue)))
                {
                    Debug.Log($"No {typeName} component");
                    return false;
                }
            }
            return true;
        }

        private bool CheckForExcludedComponents(List<ComponentDefinition> selectedComponents)
        {
            for (int i = 0; i < selectedComponents.Count - 1; i++)
            {
                for (int j = 1; j < selectedComponents.Count; j++)
                {
                    if (i == j)
                        continue;

                    if (selectedComponents[i].ExcludedComponents.Contains(selectedComponents[j]))
                    {
                        Debug.Log($"Component {selectedComponents[i].Name} have excluded component: {selectedComponents[j].Name}");
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CheckForDeathReason(List<ComponentDefinition> selectedComponents)
        {
            DeathType deathType = _currentQuest.DeathType;
            foreach (var component in selectedComponents)
            {
                if (component.IsDeathReasonExcluded && component.ExcludedDeathReasonType == deathType)
                {
                    Debug.Log($"Component {component.Name} have excluded death reason: {deathType.ToString()}");
                    return false;
                }
            }

            return true;
        }
        
        private bool CheckForAgeExcludes(List<ComponentDefinition> selectedComponents)
        {
            AgeType ageType = _currentQuest.AgeType;
            foreach (var component in selectedComponents)
            {
                if (component.IsAgeExcluded && component.ExcludedAgeType == ageType)
                {
                    Debug.Log($"Component {component.Name} have excluded death reason: {ageType.ToString()}");
                    return false;
                }
            }

            return true;
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
            _questPresenter.OnQuestAdded += SetQuest;
        }

        private void OnDestroy()
        {
            _clearTable.onClick.RemoveListener(ClearTable);
            _startRitual.onClick.RemoveListener(StartRitual);
            _questPresenter.OnQuestAdded -= SetQuest;
        }
    }
}
