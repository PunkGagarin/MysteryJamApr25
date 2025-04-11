using UnityEngine;

namespace Jam.Scripts.GameplayData.Definitions
{
    public class Definition : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}
