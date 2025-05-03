using UnityEngine;

namespace Jam.Scripts.Ritual.Desk
{
    [CreateAssetMenu(fileName = "MemoryConfig", menuName = "Game Resources/Configs/Memory")]
    public class MemoryConfig : ScriptableObject
    {
        [field: SerializeField] public int ClicksAmount;
        [field: SerializeField] public float TimeToShowClick;
    }
}