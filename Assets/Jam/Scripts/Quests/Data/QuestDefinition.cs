using System.Collections.Generic;
using Jam.Scripts.GameplayData.Definitions;
using UnityEngine;

namespace Jam.Scripts.Quests.Data
{
    [CreateAssetMenu(menuName = "Definitions/Quest")]
    public class QuestDefinition : Definition
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public List<string> StartQuestKeyWords { get; private set; }
    }
}
