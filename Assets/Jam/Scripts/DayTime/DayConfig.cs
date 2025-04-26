using System;
using System.Collections.Generic;
using Jam.Scripts.Npc.Data;
using NaughtyAttributes;
using UnityEngine;

namespace Jam.Scripts.DayTime
{
    [CreateAssetMenu(fileName = "DayConfig", menuName = "Game Resources/Configs/Day")]
    public class DayConfig : ScriptableObject
    {
        [field: SerializeField, Tooltip("Списки персонажей которые приходят на каждый день")] public List<NpcList> DayNpcs { get; private set; }
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