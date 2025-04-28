using System.Collections.Generic;
using Jam.Scripts.Ritual;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;

namespace Jam.Scripts.Manual
{
    public class Page : MonoBehaviour
    {
        [SerializeField] private List<ReagentUI> _reagentsViews;
        [SerializeField] private List<ReagentDefinition> _reagentsDefinitions;

        public void Initialize(HashSet<int> unlockedReagents, HashSet<ReagentExclusion> reagentExclusions)
        {
            if (_reagentsDefinitions == null || _reagentsDefinitions.Count == 0)
                return;

            SetupReagents(unlockedReagents);
            SetupExcludedReagents(reagentExclusions);
        }

        private void SetupReagents(HashSet<int> unlockedReagents)
        {
            for (int i = 0; i < _reagentsDefinitions.Count; i++)
                _reagentsViews[i].gameObject.SetActive(unlockedReagents.Contains(_reagentsDefinitions[i].Id));
        }

        private void SetupExcludedReagents(HashSet<ReagentExclusion> reagentExclusions)
        {
            for (int i = 0; i < _reagentsDefinitions.Count; i++)
            {
                List<Sprite> excludedReagentSprites = new List<Sprite>();

                foreach (var reagentExclusion in reagentExclusions)
                {
                    if (reagentExclusion.ReagentId == _reagentsDefinitions[i].Id)
                    {
                        Sprite excludedSprite = _reagentsDefinitions[i]
                            .GetExcludedSprite(reagentExclusion.ExcludedReagentId);
                        excludedReagentSprites.Add(excludedSprite);
                    }
                }

                _reagentsViews[i].InitData(_reagentsDefinitions[i].LocalizeId, excludedReagentSprites);
            }
        }
    }
}