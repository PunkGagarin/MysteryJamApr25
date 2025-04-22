using System.Collections.Generic;
using Jam.Scripts.Npc.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jam.Scripts.DayTime
{
    public class RandomNpcController : MonoBehaviour
    {
        [SerializeField] private NpcRepository _randomNpcRepository;

        private List<NPCDefinition> _npcList;
        
        private void Awake()
        {
            RefreshNpcsFromRepository();
        }

        private void RefreshNpcsFromRepository()
        {
            _npcList = new List<NPCDefinition>(_randomNpcRepository.Definitions);
        }

        public NPCDefinition GetNpc()
        {
            if (_npcList.Count == 0)
                RefreshNpcsFromRepository();
            
            NPCDefinition npcToReturn = _npcList[Random.Range(0, _npcList.Count)];

            _npcList.Remove(npcToReturn);

            return npcToReturn;
        }
    }
}