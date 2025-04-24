using System.Collections.Generic;
using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Manual;
using Jam.Scripts.Ritual.Inventory;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Ritual
{
    public class MirrorTool : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private float _animationTime;
        [SerializeField] private Canvas _canvas;
        [Inject] private RitualController _ritualController;
        [Inject] private ManualBookItem _manual;
        [Inject] private InventoryConfig _inventoryConfig;
        [Inject] private AudioService _audioService;
        private int _charges;

        public void Buy()
        {
            _charges = _inventoryConfig.MirrorMaxCharges;
            ShowAnimation();
        }

        private void ShowAnimation() => 
            _fill.DOFillAmount(_charges/(float)_inventoryConfig.MirrorMaxCharges, _animationTime).SetEase(Ease.Linear);

        private void Awake()
        {
            _canvas.worldCamera = UnityEngine.Camera.main;
            
            _charges = 0;
            _ritualController.OnExcludedReagentsFound += CheckReagentExclusion;
            Buy();
        }

        private void OnDestroy()
        {
            _ritualController.OnExcludedReagentsFound -= CheckReagentExclusion;
        }

        private void CheckReagentExclusion(List<ReagentExclusion> excludedReagents)
        {
            if (_charges == 0)
                return;

            if (_manual.CheckReagentExclusion(excludedReagents, out ReagentExclusion excludedReagentToAdd))
            {
                _charges--;
                ShowAnimation();
                _manual.AddReagentExclusion(excludedReagentToAdd);
                _audioService.PlaySound(Sounds.foundConflict.ToString());
            }
        }
    }
}