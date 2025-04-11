using Jam.Scripts.Dialogue.Runtime.SO;
using Jam.Scripts.GameplayData.Definitions;
using UnityEngine;

namespace Jam.Scripts.Npc.Data
{
    [CreateAssetMenu(menuName = "Definitions/Npc")]
    public class NPCDefinition : Definition
    {
        [field: SerializeField] public DialogueContainerSO Dialogue { get; private set; }
        [field: SerializeField] public int BelongQuestId { get; private set; }
    }
}
