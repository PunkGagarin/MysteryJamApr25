using System.Collections.Generic;
using Jam.Scripts.Ritual;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;

namespace Jam.Scripts.Manual
{
    public class Page : MonoBehaviour
    {
        [SerializeField] private bool _isInitializable; 
        [SerializeField] private List<ReagentUI> _reagentsViews;
        [SerializeField] private List<ReagentDefinition> _reagentsDefinitions;

        public void Initialize(List<ReagentExclusion> reagentExclusions)
        {
            //todo
            
        }
        
    }
}