﻿using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Dialogue.Gameplay;
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
        [SerializeField] private Button _clearTable;
        [SerializeField] private MainDesk _desk;
        [SerializeField] private GameObject _fireflies;

        [Inject] private QuestPresenter _questPresenter;
        [Inject] private AudioService _audioService;
        [Inject] private InventoryConfig _inventoryConfig;
        [Inject] private GhostResponseEffect _ghostResponseEffect;
        [Inject] private PointerFirefly _pointerFirefly;
        [Inject] private DialogueRunner _dialogueRunner;
        [Inject] private Memory _memory;

        private Quest _currentQuest;
        public event Action OnRitual;
        public event ExcludedReagentsFound OnExcludedReagentsFound;
        public event Action OnAddReagent;
        public event Action TutorialRitual;
        public bool RitualFailedByExcludedReagents { get; private set; }
        public bool RitualFailedByMissingSexReagent { get; private set; }
        public bool RitualFailedByMissingAgeReagent { get; private set; }
        public bool RitualFailedByMissingRaceReagent { get; private set; }
        public bool RitualFailedByMissingDeathReagent { get; private set; }
        public List<ReagentExclusion> ExcludedReagents { get; private set; }
        public int Attempt { get; private set; }
        public bool CanCheckByMagnifier => _reagentFitter.OccupiedRooms >= 2;
        public bool IsAllReagentsOnTable => !_reagentFitter.HaveFreeRooms;
        private bool _isTutorialComplete;

        public bool TryAddReagent(ReagentDefinition reagentToAdd, out ReagentRoom reagentRoom)
        {
            reagentRoom = null;
            
            if (!_questPresenter.HaveAnyQuest() || _questPresenter.IsQuestComplete() || _questPresenter.IsQuestFailed())
                return false;
            
            if (!_reagentFitter.HaveFreeRooms || _reagentFitter.HaveReagent(reagentToAdd))
                return false;
            
            _reagentFitter.SetReagent(reagentToAdd, out reagentRoom);
            switch (_pointerFirefly.CurrentTarget)
            {
                case (int)TargetType.FirstReagent:
                    _pointerFirefly.ChangeTargetTo(TargetType.SecondReagent);
                    break;
                case (int)TargetType.SecondReagent:
                    _pointerFirefly.ChangeTargetTo(TargetType.ThirdReagent);
                    break;
                default:
                    _pointerFirefly.ChangeTargetTo(TargetType.DialogueBubble3);
                    break;
            }

            UpdateButtons();
            OnAddReagent?.Invoke();

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
        }
        
        private void StartRitual()
        {
            _pointerFirefly.HideTillNextTarget();
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            Attempt++;
            RitualFailedByExcludedReagents = false;
            RitualFailedByMissingSexReagent = false;
            RitualFailedByMissingAgeReagent = false;
            RitualFailedByMissingRaceReagent = false;
            RitualFailedByMissingDeathReagent = false;
            ExcludedReagents = new List<ReagentExclusion>();

            List<ReagentDefinition> selectedReagents = _desk.GetReagents();
            
            bool isComplete = CheckRitualState(selectedReagents);

            if (isComplete)
            {
                _desk.ShowRitualResult(true, _isTutorialComplete ? StartMemoryGame : () => TutorialRitual?.Invoke());
            }
            else
            {
                _desk.ShowRitualResult(false, RitualFailed);
            }

            
            if (isComplete && _currentQuest.Id == 0)
                _fireflies.SetActive(true);
        }

        public void StartMemoryGame()
        {
            _isTutorialComplete = true;
            _memory.StartMemoryGame(RitualComplete, RitualFailed);
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

        private bool CheckRitualState(List<ReagentDefinition> selectedReagents) => 
            !CheckForExcludedReagents(selectedReagents) && CheckReagentsMatches(selectedReagents);

        private bool CheckReagentsMatches(List<ReagentDefinition> selectedReagents)
        {
            bool isRitualComplete = true;
            if (!CheckForReagents(
                    selectedReagents,
                    _currentQuest.AgeType,
                    reagent => reagent.AgeType,
                    AgeType.None,
                    "Age"))
            {
                isRitualComplete = false;
                RitualFailedByMissingAgeReagent = true;
            }

            if (!CheckForReagents(
                    selectedReagents,
                    _currentQuest.SexType,
                    reagent => reagent.SexType,
                    SexType.None,
                    "Sex"))
            {
                isRitualComplete = false;
                RitualFailedByMissingSexReagent = true;
            }

            if (!CheckForReagents(
                    selectedReagents,
                    _currentQuest.RaceType,
                    reagent => reagent.RaceType,
                    RaceType.None,
                    "Race"))
            {
                isRitualComplete = false;
                RitualFailedByMissingRaceReagent = true;
            }

            if (!CheckForReagents(
                    selectedReagents,
                    _currentQuest.DeathType,
                    reagent => reagent.DeathType,
                    DeathType.None,
                    "Death"))
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
                foreach (var reagent in reagents)
                {
                    if (reagent.ReagentType.ToString().Equals(typeName, StringComparison.InvariantCultureIgnoreCase)
                        && selector(reagent).Equals(currentQuestValue))
                        return true;
                }
                Debug.Log($"No {typeName} reagent");
                return false;
            }
            return true;
        }

        public bool CheckForExcludedReagents()
        {
            List<ReagentDefinition> selectedReagents =
                _reagentFitter.GetOccupiedRooms().Select(room => room.ReagentInside).ToList();

            return CheckForExcludedReagents(selectedReagents, false);
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
                        Debug.Log($"Reagent {selectedReagents[i].Name} have excluded reagent: {selectedReagents[j].Name}");
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
