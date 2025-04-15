using System;
using Jam.Scripts.Ritual.Components;
using UnityEngine;

namespace Jam.Scripts.Quests.Data
{
    [Serializable]
    public class Quest
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public bool IsComplete { get; set; }
        [field: SerializeField] public bool IsFailed { get; set; }
        [field: SerializeField] public AgeType AgeType { get; private set; }
        [field: SerializeField] public SexType SexType { get; private set; }
        [field: SerializeField] public RaceType RaceType { get; private set; }
        [field: SerializeField] public DeathType DeathType { get; private set; }

        public Quest(QuestDefinition questDefinition)
        {
            Name = questDefinition.name;
            Id = questDefinition.Id;
            IsComplete = false;
            IsFailed = false;
            AgeType = questDefinition.AgeType;
            SexType = questDefinition.SexType;
            RaceType = questDefinition.RaceType;
            DeathType = questDefinition.DeathType;
        }
    }
}