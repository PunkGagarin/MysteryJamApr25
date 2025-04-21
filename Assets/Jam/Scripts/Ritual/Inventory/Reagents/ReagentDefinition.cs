using System.Collections.Generic;
using Jam.Scripts.GameplayData.Definitions;
using NaughtyAttributes;
using UnityEngine;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    [CreateAssetMenu(menuName = "Game Resources/Definitions/Component")]
    public class ReagentDefinition : Definition
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public Sprite Visual { get; private set; }
        [field: SerializeField] public ReagentType ReagentType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ReagentType), ReagentType.Age)] public AgeType AgeType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ReagentType), ReagentType.Sex)] public SexType SexType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ReagentType), ReagentType.Race)] public RaceType RaceType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ReagentType), ReagentType.Death)] public DeathType DeathType {get; private set; }
        [field: SerializeField] public List<ReagentDefinition> ExcludedReagents { get; private set; }
        [field: SerializeField] public bool IsDeathReasonExcluded { get; private set; }
        [field: SerializeField, ShowIf("IsDeathReasonExcluded")] public DeathType ExcludedDeathReasonType { get; private set;}
        [field: SerializeField] public bool IsAgeExcluded { get; private set; }
        [field: SerializeField, ShowIf("IsAgeExcluded")] public AgeType ExcludedAgeType { get; private set;}
    }
}
