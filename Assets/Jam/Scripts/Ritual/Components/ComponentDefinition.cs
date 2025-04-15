using System.Collections.Generic;
using Jam.Scripts.GameplayData.Definitions;
using NaughtyAttributes;
using UnityEngine;

namespace Jam.Scripts.Ritual.Components
{
    [CreateAssetMenu(menuName = "Game Resources/Definitions/Component")]
    public class ComponentDefinition : Definition
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public Sprite Visual { get; private set; }
        [field: SerializeField] public ComponentType ComponentType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ComponentType), ComponentType.Age)] public AgeType AgeType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ComponentType), ComponentType.Sex)] public SexType SexType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ComponentType), ComponentType.Race)] public RaceType RaceType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ComponentType), ComponentType.Death)] public DeathType DeathType {get; private set; }
        [field: SerializeField] public List<ComponentDefinition> ExcludedComponents { get; private set; }
        [field: SerializeField] public bool IsDeathReasonExcluded { get; private set; }
        [field: SerializeField, ShowIf("IsDeathReasonExcluded")] public DeathType ExcludedDeathReasonType { get; private set;}
        [field: SerializeField] public bool IsAgeExcluded { get; private set; }
        [field: SerializeField, ShowIf("IsAgeExcluded")] public AgeType ExcludedAgeType { get; private set;}
    }
}
