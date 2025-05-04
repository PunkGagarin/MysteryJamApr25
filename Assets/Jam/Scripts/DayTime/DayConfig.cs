using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Npc.Data;
using Jam.Scripts.Ritual.Desk;
using NaughtyAttributes;
using UnityEngine;

namespace Jam.Scripts.DayTime
{
    [CreateAssetMenu(fileName = "DayConfig", menuName = "Game Resources/Configs/Day")]
    public class DayConfig : ScriptableObject
    {
        [field: SerializeField, Tooltip("Списки персонажей которые приходят на каждый день")] public List<NpcList> DayNpcs { get; private set; }
        [field: SerializeField] public List<MemoryDifficultByDay> MemoryDifficultByDays { get; private set; }

        public MemoryConfig GetMemoryConfig(int currentDay) => 
            (from memoryDifficultByDay in MemoryDifficultByDays where memoryDifficultByDay.StartDayNum == currentDay select memoryDifficultByDay.MemoryConfig).FirstOrDefault();
    }

    [Serializable]
    public class MemoryDifficultByDay
    {
        [field: SerializeField, Tooltip("С какого дня начинает действовать эта сложность мемори игры")] public int StartDayNum { get; set; }
        [field: SerializeField] public MemoryConfig MemoryConfig { get; set; }
    }

    [Serializable]
    public class NpcList
    {
        [field: SerializeField] public List<DayNpc> Npcs { get; private set; }
    }

    [Serializable]
    public class DayNpc
    {
        [field: SerializeField] public bool IsRandomNpc { get; set; }
        [field: SerializeField, AllowNesting, HideIf("IsRandomNpc")] public NPCDefinition Npc { get; private set; }
    }
}