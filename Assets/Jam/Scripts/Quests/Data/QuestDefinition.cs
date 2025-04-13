using System.Collections.Generic;
using Jam.Scripts.GameplayData.Definitions;
using Jam.Scripts.Ritual;
using UnityEngine;

namespace Jam.Scripts.Quests.Data
{
    [CreateAssetMenu(menuName = "Game Resources/Definitions/Quest")]
    public class QuestDefinition : Definition
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public List<string> StartQuestKeyWords { get; private set; }
        [field: SerializeField] public RitualDefinition Ritual { get; private set; }
    }
}
