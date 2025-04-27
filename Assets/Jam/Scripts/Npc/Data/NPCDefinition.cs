using Jam.Scripts.Dialogue.Runtime.SO;
using Jam.Scripts.GameplayData.Definitions;
using Jam.Scripts.Quests.Data;
using UnityEngine;

namespace Jam.Scripts.Npc.Data
{
    [CreateAssetMenu(menuName = "Game Resources/Definitions/Npc")]
    public class NPCDefinition : Definition
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public DialogueContainerSO Dialogue { get; private set; }
        [field: SerializeField] public QuestDefinition Quest { get; private set; }
        [field: SerializeField] public Sprite Visual { get; private set; } 
    }
}
