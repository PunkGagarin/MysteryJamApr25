using System;
using System.Collections.Generic;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Runtime.Events.SO
{
    [CreateAssetMenu(menuName = "Game Resources/Dialogue/Events/Add Reagents")]
    public class AddReagentsEvent : DialogueEventSO
    {
        [SerializeField] private List<ReagentDefinition> _reagentsToAdd;
        
        public event Action<List<ReagentDefinition>> OnReagentsAdded;

        public override void RunEvent() => 
            OnReagentsAdded?.Invoke(_reagentsToAdd);
    }
}