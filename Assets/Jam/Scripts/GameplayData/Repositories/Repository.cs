using System.Collections.Generic;
using Jam.Scripts.GameplayData.Definitions;
using UnityEngine;

namespace Jam.Scripts.GameplayData.Repositories
{
    public abstract class Repository<T> : ScriptableObject where T : Definition
    {
        [field: SerializeField] public List<T> Definitions { get; private set; }
    }
}
