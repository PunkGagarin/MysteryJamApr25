using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Ritual;
using UnityEngine;

namespace Jam.Scripts.Manual
{
    public class ReagentsContainer : MonoBehaviour
    {
        [SerializeField] private List<ReagentTypeContainer> _typeContainers;

        private void Awake() => 
            _typeContainers.ForEach(container => container.gameObject.SetActive(false));

        public void SetupReagents(HashSet<int> unlockedReagents, HashSet<ReagentExclusion> reagentExclusions)
        {
            foreach (var typeContainer in _typeContainers)
            {
                if (typeContainer.ContainReagents(unlockedReagents))
                {
                    typeContainer.gameObject.SetActive(true);
                    typeContainer.SetupReagents(unlockedReagents, reagentExclusions);
                }
            }
        }

        public bool Contains(HashSet<int> unlockedReagents) => 
            _typeContainers.Any(typeContainer => typeContainer.ContainReagents(unlockedReagents));
    }
}