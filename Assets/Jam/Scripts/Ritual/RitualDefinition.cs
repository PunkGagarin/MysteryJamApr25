using System.Collections.Generic;
using Jam.Scripts.GameplayData.Definitions;
using Jam.Scripts.Ritual.Components;
using UnityEngine;

namespace Jam.Scripts.Ritual
{
    [CreateAssetMenu(menuName = "Game Resources/Definitions/Ritual")]
    public class RitualDefinition : Definition
    {
        [field: SerializeField] public List<ComponentDefinition> Components { get; private set; }
    }
}
