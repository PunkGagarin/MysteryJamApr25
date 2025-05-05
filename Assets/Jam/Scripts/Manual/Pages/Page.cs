using System.Collections.Generic;
using Jam.Scripts.Ritual;
using UnityEngine;

namespace Jam.Scripts.Manual.Pages
{
    public class Page : MonoBehaviour
    {
        [SerializeField] private List<ReagentsContainer> _reagentsContainer;

        public void Initialize(HashSet<int> unlockedReagents, HashSet<ReagentExclusion> reagentExclusions)
        {
            if (_reagentsContainer == null || _reagentsContainer.Count == 0)
                return;

            foreach (var reagentsContainer in _reagentsContainer)
            {
                if (reagentsContainer.Contains(unlockedReagents))
                    reagentsContainer.SetupReagents(unlockedReagents, reagentExclusions);
            }
        }
    }
}