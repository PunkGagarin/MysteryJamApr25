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
        public delegate void ExcludedReagentsFound(List<ReagentExclusion> excludedReagents);
        
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
        public event ExcludedReagentsFound OnExcludedReagentsFound;
        public bool RitualFailedByExcludedReagents { get; private set; }
        public bool RitualFailedByMissingSexReagent { get; private set; }
        public bool RitualFailedByMissingAgeReagent { get; private set; }
        public bool RitualFailedByMissingRaceReagent { get; private set; }
        public bool RitualFailedByMissingDeathReagent { get; private set; }
        public List<ReagentExclusion> ExcludedReagents { get; private set; }

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
            RitualFailedByExcludedReagents = false;
            RitualFailedByMissingSexReagent = false;
            RitualFailedByMissingAgeReagent = false;
            RitualFailedByMissingRaceReagent = false;
            RitualFailedByMissingDeathReagent = false;
            ExcludedReagents = new List<ReagentExclusion>();

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

            if (CheckForExcludedReagents(selectedComponents))
                return false;

            return CheckReagentsMatches(selectedComponents);
        }

        private bool CheckReagentsMatches(List<ReagentDefinition> selectedComponents)
        {
            bool isRitualComplete = true;
            if (!CheckForReagents(
                    selectedComponents,
                    _currentQuest.AgeType,
                    reagent => reagent.AgeType,
                    AgeType.None,
                    "age"))
            {
                isRitualComplete = false;
                RitualFailedByMissingAgeReagent = true;
            }

            if (!CheckForReagents(
                    selectedComponents,
                    _currentQuest.SexType,
                    reagent => reagent.SexType,
                    SexType.None,
                    "sex"))
            {
                isRitualComplete = false;
                RitualFailedByMissingSexReagent = true;
            }

            if (!CheckForReagents(
                    selectedComponents,
                    _currentQuest.RaceType,
                    reagent => reagent.RaceType,
                    RaceType.None,
                    "race"))
            {
                isRitualComplete = false;
                RitualFailedByMissingRaceReagent = true;
            }

            if (!CheckForReagents(
                    selectedComponents,
                    _currentQuest.DeathType,
                    reagent => reagent.DeathType,
                    DeathType.None,
                    "death"))
            {
                isRitualComplete = false;
                RitualFailedByMissingDeathReagent = true;
            }

            return isRitualComplete;
        }
        private bool CheckForReagents<T>(
            List<ReagentDefinition> reagents,
            T currentQuestValue,
            Func<ReagentDefinition, T> selector,
            T noneValue,
            string typeName)
            where T : Enum
        {
            if (!currentQuestValue.Equals(noneValue))
            {
                if (reagents.All(reagent => !selector(reagent).Equals(currentQuestValue)))
                {
                    Debug.Log($"No {typeName} component");
                    return false;
                }
            }
            return true;
        }

        private bool CheckForExcludedReagents(List<ReagentDefinition> selectedReagents, bool withSignal = true)
        {
            bool haveExcludedReagents = false; 
            for (int i = 0; i < selectedReagents.Count; i++)
            {
                for (int j = 0; j < selectedReagents.Count; j++)
                {
                    if (i == j) 
                        continue;
                    
                    if (selectedReagents[i].ExcludedReagents.Contains(selectedReagents[j]))
                    {
                        ExcludedReagents.Add(new ReagentExclusion (selectedReagents[i].Id, selectedReagents[j].Id));
                        Debug.Log($"Component {selectedReagents[i].Name} have excluded component: {selectedReagents[j].Name}");
                        haveExcludedReagents = true;
                        RitualFailedByExcludedReagents = true;
                    }
                }
            }

            if (haveExcludedReagents && withSignal) 
                OnExcludedReagentsFound?.Invoke(ExcludedReagents);

            return haveExcludedReagents;
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

        private void UpdateButtons()
        {
            bool isActive = _reagentRooms.Any(component => !component.IsFree);
            _clearTable.gameObject.SetActive(isActive);
            _startRitual.gameObject.SetActive(isActive);
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

    public class ReagentExclusion
    {
        public int ReagentId { get; private set; }
        public int ExcludedReagentId { get; private set; }

        public ReagentExclusion(int reagentId, int excludedReagentId)
        {
            ReagentId = reagentId;
            ExcludedReagentId = excludedReagentId;
        }

        public override bool Equals(object obj)
        {
            if (obj is ReagentExclusion other)
            {
                return ReagentId == other.ReagentId &&
                       ExcludedReagentId == other.ExcludedReagentId;
            }
            return false;
        }

        protected bool Equals(ReagentExclusion other)
        {
            return ReagentId == other.ReagentId && ExcludedReagentId == other.ExcludedReagentId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReagentId, ExcludedReagentId);
        }
    }
}
