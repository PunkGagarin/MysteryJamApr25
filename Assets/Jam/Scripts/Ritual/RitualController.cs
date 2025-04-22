using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.PostProcessing;
using Jam.Scripts.Quests;
using Jam.Scripts.Quests.Data;
using Jam.Scripts.Ritual.Inventory;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Ritual
{
    public class RitualController : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ReagentRoom _reagentRoomPrefab;
        [SerializeField] private Transform _reagentsGroup;
        [SerializeField] private Button _startRitual;
        [SerializeField] private Button _clearTable;
        
        [Inject] private QuestPresenter _questPresenter;
        [Inject] private QuestRepository _questRepository;
        [Inject] private AudioService _audioService;
        [Inject] private InventoryConfig _inventoryConfig;
        [Inject] private InventorySystem _inventorySystem;
        [Inject] private GhostResponseEffect _ghostResponseEffect;
        
        private List<ReagentRoom> _reagentRooms = new ();

        private Quest _currentQuest;
        public event Action OnRitual; 

        public int Attempt { get; private set; }

        public bool TryAddComponent(ReagentDefinition reagentToAdd, out ReagentRoom reagentRoom)
        {
            reagentRoom = null;
            
            if (!_questPresenter.HaveAnyQuest() || _questPresenter.IsQuestComplete() || _questPresenter.IsQuestFailed())
                return false;
            
            if (_reagentRooms.All(componentRoom => !componentRoom.IsFree))
                return false;
            
            reagentRoom = _reagentRooms.First(componentRoom => componentRoom.IsFree);  
            
            UpdateButtons();

            reagentRoom.SetReagent(reagentToAdd);
            return true;
        }

        private void SetQuest(Quest quest)
        {
            _currentQuest = quest;
            Attempt = 0;
        }

        private void OnClearTableButton() =>
            ClearTable(false);
        
        private void ClearTable(bool consumeReagents)
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            
            foreach (var componentRoom in _reagentRooms.Where(componentRoom => !componentRoom.IsFree)) 
                componentRoom.ReleaseReagent(consumeReagents);

            UpdateButtons();
        }
        
        private void StartRitual()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            Attempt++;

            List<ReagentDefinition> selectedComponents =
                _reagentRooms.Where(component => !component.IsFree).Select(component => component.ReagentInside).ToList();
            
            bool areComplete = CheckRitualState(selectedComponents);
            
            if (areComplete)
            {
                Debug.Log($"Ritual OK");
                _audioService.PlaySound(Sounds.whisperingGhosts.ToString());
                _ghostResponseEffect.ToggleEffect();
                _questPresenter.SetComplete();
            }
            else
            {
                Debug.Log($"Ritual failed");
                _audioService.PlaySound(Sounds.ritualFailed.ToString());
                if (Attempt >= _inventoryConfig.RitualAttemptsToFail)
                {
                    Debug.Log("Quest failed");
                    _questPresenter.SetFail();
                }
            }

            OnRitual?.Invoke();

            ClearTable(true);
            UpdateButtons();
        }
        
        private bool CheckRitualState(List<ReagentDefinition> selectedComponents)
        {
            if (!CheckForDeathReason(selectedComponents)) 
                return false;
            
            if (!CheckForAgeExcludes(selectedComponents)) 
                return false;

            if (!CheckForExcludedComponents(selectedComponents))
                return false;

            return CheckComponentMatches(selectedComponents);
        }

        private bool CheckComponentMatches(List<ReagentDefinition> selectedComponents) =>
            CheckForComponent(
                selectedComponents,
                _currentQuest.AgeType,
                reagent => reagent.AgeType,
                AgeType.None,
                "age")
            && CheckForComponent(
                selectedComponents,
                _currentQuest.SexType,
                reagent => reagent.SexType,
                SexType.None,
                "sex")
            && CheckForComponent(
                selectedComponents,
                _currentQuest.RaceType,
                reagent => reagent.RaceType,
                RaceType.None,
                "race")
            && CheckForComponent(
                selectedComponents,
                _currentQuest.DeathType,
                reagent => reagent.DeathType,
                DeathType.None,
                "death");

        private bool CheckForComponent<T>(
            List<ReagentDefinition> reagents,
            T currentQuestValue,
            Func<ReagentDefinition, T> selector,
            T noneValue,
            string typeName)
            where T : Enum
        {
            if (!currentQuestValue.Equals(noneValue))
            {
                if (reagents.Any(reagent => !selector(reagent).Equals(currentQuestValue)))
                {
                    Debug.Log($"No {typeName} component");
                    return false;
                }
            }
            return true;
        }

        private bool CheckForExcludedComponents(List<ReagentDefinition> selectedComponents)
        {
            for (int i = 0; i < selectedComponents.Count - 1; i++)
            {
                for (int j = i + 1; j < selectedComponents.Count; j++)
                {
                    if (selectedComponents[i].ExcludedReagents.Contains(selectedComponents[j]))
                    {
                        Debug.Log($"Component {selectedComponents[i].Name} have excluded component: {selectedComponents[j].Name}");
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CheckForDeathReason(List<ReagentDefinition> selectedComponents)
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
        
        private bool CheckForAgeExcludes(List<ReagentDefinition> selectedComponents)
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
            bool isActive = _reagentRooms.Any(component => !component.IsFree);
            _clearTable.gameObject.SetActive(isActive);
            _startRitual.gameObject.SetActive(isActive);
        }

        private void SetupRooms()
        {
            for (int i = 0; i < _inventoryConfig.RoomsForRitual; i++)
            {
                ReagentRoom room = Instantiate(_reagentRoomPrefab, _reagentsGroup);
                room.Initialize(_inventorySystem);
                _reagentRooms.Add(room);
                room.OnRoomChanged += UpdateButtons;
            }
        }

        private void Awake()
        {
            _canvas.worldCamera = UnityEngine.Camera.main;
            
            UpdateButtons();
            SetupRooms();
            _clearTable.onClick.AddListener(OnClearTableButton);
            _startRitual.onClick.AddListener(StartRitual);
            _questPresenter.OnQuestAdded += SetQuest;
        }

        private void OnDestroy()
        {
            _clearTable.onClick.RemoveListener(OnClearTableButton);
            _startRitual.onClick.RemoveListener(StartRitual);
            _questPresenter.OnQuestAdded -= SetQuest;
            
            foreach (var room in _reagentRooms) 
                room.OnRoomChanged -= UpdateButtons;
        }
    }
}
