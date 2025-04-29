using Jam.Scripts.GameplayData.Definitions;
using UnityEngine;

namespace Jam.Scripts.Ritual.Tools
{
    [CreateAssetMenu(menuName = "Game Resources/Definitions/Tool")]
    public class ToolDefinition : Definition
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public int Charges { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public Sprite Visual { get; private set; }
    }
}