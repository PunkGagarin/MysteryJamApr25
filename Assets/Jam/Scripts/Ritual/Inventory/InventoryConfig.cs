using System.Collections.Generic;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;

namespace Jam.Scripts.Ritual.Inventory
{
    [CreateAssetMenu(menuName = "Game Resources/Configs/Inventory")]
    public class InventoryConfig : ScriptableObject
    {
        [field: SerializeField] public float ReagentAnimationTime { get; private set; }
        [field: SerializeField] public int RoomsForRitual { get; private set; }
        [field: SerializeField] public int MaxReagentAmount { get; private set; }
        [field: SerializeField] public List<ReagentDefinition> StartReagents { get; private set; }
        [field: SerializeField] public int RitualAttemptsToFail { get; private set; }
    }
}