using System;
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

        public Quest(QuestDefinition questDefinition)
        {
            Name = questDefinition.name;
            Id = questDefinition.Id;
            IsComplete = false;
            IsFailed = false;
        }
    }
}