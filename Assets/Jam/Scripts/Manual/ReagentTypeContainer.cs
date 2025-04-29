using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Ritual;
using UnityEngine;

namespace Jam.Scripts.Manual
{
    public class ReagentTypeContainer : MonoBehaviour
    {
        [SerializeField] private List<ReagentUI> _reagents;
        
        public void SetupReagents(HashSet<int> unlockedReagents, HashSet<ReagentExclusion> reagentExclusions)
        {
            foreach (var reagent in _reagents) 
                reagent.gameObject.SetActive(unlockedReagents.Contains(reagent.Definition.Id));

            SetupExcludedReagents(reagentExclusions);
        }

        public bool ContainReagents(HashSet<int> unlockedReagents) => 
            _reagents.Any(reagent => unlockedReagents.Contains(reagent.Definition.Id));

        private void SetupExcludedReagents(HashSet<ReagentExclusion> reagentExclusions)
        {
            foreach (var reagent in _reagents)
            {
                List<Sprite> excludedReagentSprites =
                    (from reagentExclusion 
                            in reagentExclusions 
                        where reagentExclusion.ReagentId == reagent.Definition.Id 
                        select reagent.Definition.GetExcludedSprite(reagentExclusion.ExcludedReagentId))
                    .ToList();

                reagent.InitData(excludedReagentSprites);
            }
        }
    }
}