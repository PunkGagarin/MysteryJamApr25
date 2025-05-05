using System;
using System.Collections.Generic;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.GameplayData.Definitions;
using NaughtyAttributes;
using UnityEngine;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    [CreateAssetMenu(menuName = "Game Resources/Definitions/Reagent")]
    public class ReagentDefinition : Definition
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public Sprite Visual { get; private set; }
        [field: SerializeField] public Sprite ManualIcon { get; private set; }
        [field: SerializeField] public Sounds ClickClip { get; private set; }
        [field: SerializeField] public ReagentType ReagentType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ReagentType), ReagentType.Age)] public AgeType AgeType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ReagentType), ReagentType.Sex)] public SexType SexType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ReagentType), ReagentType.Race)] public RaceType RaceType {get; private set; }
        [field: SerializeField, ShowIf(nameof(ReagentType), ReagentType.Death)] public DeathType DeathType {get; private set; }
        [field: SerializeField] public List<ReagentDefinition> ExcludedReagents { get; private set; }

        [field: SerializeField] public int Cost { get; private set; }
        
        public int CurrentAmount { get; set; }

        public Sprite GetManualExcludedIcon(int reagentExclusionExcludedReagentId)
        {
            foreach (var excludedReagent in ExcludedReagents)
            {
                if (excludedReagent.Id == reagentExclusionExcludedReagentId)
                    return excludedReagent.ManualIcon;
            }

            Debug.LogError($"Trying to get sprite for excluded reagent with id {reagentExclusionExcludedReagentId}, but it doesn't exist in excluded reagents");
            return null;
        }

        private void OnValidate()
        {
            switch (ReagentType)
            {
                case ReagentType.None:
                    AgeType = AgeType.None;
                    SexType = SexType.None;
                    RaceType = RaceType.None;
                    DeathType = DeathType.None;
                    break;
                case ReagentType.Age:
                    SexType = SexType.None;
                    RaceType = RaceType.None;
                    DeathType = DeathType.None;
                    break;
                case ReagentType.Sex:
                    AgeType = AgeType.None;
                    RaceType = RaceType.None;
                    DeathType = DeathType.None;
                    break;
                case ReagentType.Race:
                    AgeType = AgeType.None;
                    SexType = SexType.None;
                    DeathType = DeathType.None;
                    break;
                case ReagentType.Death:
                    AgeType = AgeType.None;
                    SexType = SexType.None;
                    RaceType = RaceType.None;
                    break;
            }
        }
    }
}
