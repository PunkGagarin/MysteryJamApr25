using Jam.Scripts.GameplayData.Definitions;
using UnityEngine;

namespace Jam.Scripts.Ritual.Components
{
    [CreateAssetMenu(menuName = "Game Resources/Definitions/Component")]
    public class ComponentDefinition : Definition
    {
        [field: SerializeField] public Sprite Visual { get; private set; }
    }
}
