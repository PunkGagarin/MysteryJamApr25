using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.PostProcessing;
using Jam.Scripts.Quests;
using Jam.Scripts.Quests.Data;
using Jam.Scripts.Ritual.Desk;
using Jam.Scripts.Ritual.Inventory;
using Jam.Scripts.Ritual.Inventory.Reagents;
using Jam.Scripts.VFX;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Ritual
{
    public class RitualController : MonoBehaviour
    {
        public delegate void ExcludedReagentsFound(List<ReagentExclusion> excludedReagents);
        
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ReagentFitter _reagentFitter;
        //[SerializeField] private Button _startRitual;
        [SerializeField] private Button _clearTable;
        [SerializeField] private MainDesk _desk;
        [SerializeField] private Memory _memory;
        
        [Inject] private QuestPresenter _questPresenter;
        [Inject] private AudioService _audioService;
        [Inject] private InventoryConfig _inventoryConfig;
        [Inject] private GhostResponseEffect _ghostResponseEffect;
        [Inject] private PointerFirefly _pointerFirefly;
        
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

        public bool CanCheckByMagnifier => _reagentFitter.OccupiedRooms >= 2;

        public bool TryAddComponent(ReagentDefinition reagentToAdd, out ReagentRoom reagentRoom)
        {
            reagentRoom = null;
            
            if (!_questPresenter.HaveAnyQuest() || _questPresenter.IsQuestComplete() || _questPresenter.IsQuestFailed())
                return false;
            
            if (!_reagentFitter.HaveFreeRooms || _reagentFitter.HaveReagent(reagentToAdd))
                return false;
            
            _reagentFitter.SetReagent(reagentToAdd, out reagentRoom);
            _pointerFirefly.ChangeTargetTo(TargetType.Table);

            UpdateButtons();

            return true;
        }

        private void SetQuest(Quest quest)
        {
            _currentQuest = quest;
            Attempt = 0;
            _desk.Shuffle();
        }

        private void OnClearTableButton() => 
            ClearTable(false);

        private void ClearTable(bool consumeReagents)
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());

            _reagentFitter.ReleaseRooms(consumeReagents);

            UpdateButtons();
            _desk.ClearTable();
            _pointerFirefly.ChangeTargetTo(TargetType.None);
        }
        
        private void StartRitual()
        {
            _pointerFirefly.ChangeTargetTo(TargetType.None);
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            Attempt++;
            RitualFailedByExcludedReagents = false;
            RitualFailedByMissingSexReagent = false;
            RitualFailedByMissingAgeReagent = false;
            RitualFailedByMissingRaceReagent = false;
            RitualFailedByMissingDeathReagent = false;
            ExcludedReagents = new List<ReagentExclusion>();

            List<ReagentDefinition> selectedComponents = _desk.GetReagents();
            
            bool isComplete = CheckRitualState(selectedComponents);

            if (isComplete)
                _desk.ShowRitualResult(true, () => _memory.StartMemoryGame(RitualComplete, RitualFailed));
            else
                _desk.ShowRitualResult(false, RitualFailed);

        }

        private void RitualFailed()
        {
            Debug.Log($"Ritual failed");
            _audioService.PlaySound(Sounds.ritualFailed.ToString());
            if (Attempt >= _inventoryConfig.RitualAttemptsToFail)
            {
                Debug.Log("Quest failed");
                _questPresenter.SetFail();
            }
            
            RitualEnds();
        }

        private void RitualEnds()
        {
            UpdateButtons();
            ClearTable(true);
            _desk.HideResults();
            OnRitual?.Invoke();
        }

        private void RitualComplete()
        {
            Debug.Log($"Ritual OK");
            _audioService.PlaySound(Sounds.whisperingGhosts.ToString());
            _ghostResponseEffect.ToggleEffect();
            _questPresenter.SetComplete();

            RitualEnds();
        }

        private bool CheckRitualState(List<ReagentDefinition> selectedComponents) => 
            !CheckForExcludedReagents(selectedComponents) && CheckReagentsMatches(selectedComponents);

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

        public bool CheckForExcludedReagents()
        {
            List<ReagentDefinition> selectedComponents =
                _reagentFitter.GetOccupiedRooms().Select(room => room.ReagentInside).ToList();

            return CheckForExcludedReagents(selectedComponents, false);
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

        private void OnDisksChanged()
        {
            if (_desk.IsAllDiskOccupied)
                StartRitual();
            
            UpdateButtons();
        }
        
        private void UpdateButtons()
        {
            _clearTable.gameObject.SetActive(_reagentFitter.OccupiedRooms > 0 && !_desk.IsAllDiskOccupied);
        }

        private void Awake()
        {
            _canvas.worldCamera = UnityEngine.Camera.main;
            
            UpdateButtons();
            _clearTable.onClick.AddListener(OnClearTableButton);
            //_startRitual.onClick.AddListener(StartRitual);
            _questPresenter.OnQuestAdded += SetQuest;
            _desk.OnAnyDiskChanged += OnDisksChanged;
            _reagentFitter.OnAnyRoomChanged += UpdateButtons;
        }

        private void OnDestroy()
        {
            _clearTable.onClick.RemoveListener(OnClearTableButton);
            //_startRitual.onClick.RemoveListener(StartRitual);
            _questPresenter.OnQuestAdded -= SetQuest;
            _desk.OnAnyDiskChanged -= OnDisksChanged;
            _reagentFitter.OnAnyRoomChanged -= UpdateButtons;
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

        protected bool Equals(ReagentExclusion other) => 
            ReagentId == other.ReagentId && ExcludedReagentId == other.ExcludedReagentId;

        public override int GetHashCode()
        {
            return HashCode.Combine(ReagentId, ExcludedReagentId);
        }
    }
}
