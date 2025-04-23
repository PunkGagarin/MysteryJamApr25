using System.Collections.Generic;
using Jam.Scripts.Manual;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Ritual
{
    public class MirrorTool : MonoBehaviour
    {
        [Inject] private RitualController _ritualController;
        [Inject] private ManualBookItem _manual;
        private int _charges;

        private void Awake()
        {
            _charges = 50;
            _ritualController.OnExcludedReagentsFound += CheckReagentExclusion;
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
                _manual.AddReagentExclusion(excludedReagentToAdd);
                //TODO: VFX SFX
            }
        }
    }
}