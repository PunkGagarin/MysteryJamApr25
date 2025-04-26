using System.Collections.Generic;
using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Manual;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Ritual.Tools
{
    public class MirrorTool : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private float _animationTime;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ToolDefinition _mirrorDefinition;
        [Inject] private RitualController _ritualController;
        [Inject] private ManualBookItem _manual;
        [Inject] private AudioService _audioService;
        private int _charges;
        
        public ToolDefinition Definition => _mirrorDefinition;
        public bool IsUnlocked { get; set; }
        
        public void ResetCharges()
        {
            _charges = _mirrorDefinition.Charges;
            ShowUpdateCharges();
        }

        private void ShowUpdateCharges() => 
            _fill.DOFillAmount(_charges/(float)_mirrorDefinition.Charges, _animationTime).SetEase(Ease.Linear);

        private void Awake()
        {
            _canvas.worldCamera = UnityEngine.Camera.main;
            
            _charges = 0;
            _ritualController.OnExcludedReagentsFound += CheckReagentExclusion;
            ResetCharges();
        }

        private void OnDestroy()
        {
            _ritualController.OnExcludedReagentsFound -= CheckReagentExclusion;
        }

        private void CheckReagentExclusion(List<ReagentExclusion> excludedReagents)
        {
            if (_charges == 0 || !IsUnlocked)
                return;

            if (_manual.CheckReagentExclusion(excludedReagents, out ReagentExclusion excludedReagentToAdd))
            {
                _charges--;
                ShowUpdateCharges();
                _manual.AddReagentExclusion(excludedReagentToAdd);
                _audioService.PlaySound(Sounds.foundConflict.ToString());
            }
        }
    }
}