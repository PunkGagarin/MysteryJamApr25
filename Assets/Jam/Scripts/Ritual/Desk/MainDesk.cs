using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;
using Random = System.Random;

namespace Jam.Scripts.Ritual.Desk
{
    public class MainDesk : MonoBehaviour
    {
        [SerializeField] private List<Disk> _disks;
        [SerializeField] private SpriteRenderer _rightPattern, _wrongPattern;
        [SerializeField] private Transform _patternMask;
        [SerializeField] private float _patternShowAnimationTime;
        [SerializeField] private float _patternScale;

        public event Action OnAnyDiskChanged;

        public int OccupiedDisks => 
            _disks.Count(disk => disk.ReagentInside != null);

        public bool IsAllDiskOccupied => _disks.All(disk => disk.ReagentInside != null);

        public void ClearTable()
        {
            foreach (var disk in _disks) 
                disk.ClearReagent();
        }

        public List<ReagentDefinition> GetReagents() => 
            (from disk in _disks where disk.ReagentInside != null select disk.ReagentInside).ToList();

        public void Shuffle()
        {
            var rng = new Random();

            var numbers = Enumerable.Range(1, _disks.Count).ToList();

            numbers = numbers.OrderBy(n => rng.Next()).ToList();

            for (int i = 0; i < _disks.Count; i++)
                _disks[i].SetType(numbers[i]);
        }

        public void ShowRitualResult(bool isPositive, Action onShowComplete)
        {
            _disks.ForEach(disk => disk.IsLocked = true);
            
            _rightPattern.enabled = isPositive;
            _wrongPattern.enabled = !isPositive;
            _patternMask
                .DOScale(_patternScale, _patternShowAnimationTime)
                .SetEase(Ease.Linear)
                .OnComplete(() => onShowComplete?.Invoke());
        }

        public void HideResults()
        {
            _disks.ForEach(disk => disk.IsLocked = false);
            
            _patternMask
                .DOScale(0, _patternShowAnimationTime)
                .SetEase(Ease.Linear);
        }
        private void Awake()
        {
            foreach (Disk disk in _disks) 
                disk.OnDiskChanged += DisksChanged;
        }

        private void OnDestroy()
        {
            foreach (Disk disk in _disks) 
                disk.OnDiskChanged -= DisksChanged;
        }

        private void DisksChanged() => 
            OnAnyDiskChanged?.Invoke();
    }
}