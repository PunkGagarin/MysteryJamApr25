using System.Collections.Generic;
using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Manual;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Ritual.Tools
{
    public class MirrorRitualTool : RitualTool
    {
        [Inject] private RitualController _ritualController;
        [Inject] private ManualBookItem _manual;
        [Inject] private AudioService _audioService;
        
        private void Start()
        {
            _ritualController.OnExcludedReagentsFound += CheckReagentExclusion;
        }

        private void OnDestroy()
        {
            _ritualController.OnExcludedReagentsFound -= CheckReagentExclusion;
        }

        private void CheckReagentExclusion(List<ReagentExclusion> excludedReagents)
        {
            if (Charges == 0 || !IsUnlocked)
                return;

            if (_manual.CheckReagentExclusion(excludedReagents, out ReagentExclusion excludedReagentToAdd))
            {
                Charges--;
                ShowUpdateCharges();
                Visual.DOColor(Color.white, 1f)
                    .OnComplete(() => Visual.DOColor(Color.white, 1f)
                        .OnComplete(() => Visual.DOColor(Color.clear, 1f)));
                _manual.AddReagentExclusion(excludedReagentToAdd);
            }
        }
    }
}