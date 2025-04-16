using System;
using System.Collections.Generic;
using Jam.Scripts.Npc.Data;
using UnityEngine;

namespace Jam.Scripts.DayTime
{
    [CreateAssetMenu(fileName = "DayConfig", menuName = "Game Resources/DayConfig")]
    public class DayConfig : ScriptableObject
    {
        [field: SerializeField, Tooltip("Время в секундах до конца дня")] public int DayLength { get; private set; }
        [field: SerializeField, Tooltip("Списки персонажей которые приходят на каждый день")] public List<DayNpc> DayNpcs { get; private set; }
    }

    [Serializable]
    public class DayNpc
    {
        [field: SerializeField] public List<NPCDefinition> Npcs { get; private set; }
    }
}