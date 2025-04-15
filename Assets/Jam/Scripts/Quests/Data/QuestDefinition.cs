using Jam.Scripts.GameplayData.Definitions;
using Jam.Scripts.Ritual.Components;
using UnityEngine;

namespace Jam.Scripts.Quests.Data
{
    [CreateAssetMenu(menuName = "Game Resources/Definitions/Quest")]
    public class QuestDefinition : Definition
    {
        [field: SerializeField] public int Id { get; private set; }

        [field: SerializeField] public SexType SexType { get; private set; }
        [field: SerializeField] public AgeType AgeType { get; private set; }
        [field: SerializeField] public RaceType RaceType { get; private set; }
        [field: SerializeField] public DeathType DeathType { get; private set; } 
    }
}
