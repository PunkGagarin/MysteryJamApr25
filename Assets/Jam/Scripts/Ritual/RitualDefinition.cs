using System;
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

    [Serializable]
    public class RitualItem
    {
        [field: SerializeField] public ComponentType ComponentType { get; private set; }
        [field: SerializeField] public int Amount { get; private set; } 
    }
}
