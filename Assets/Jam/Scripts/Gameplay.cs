using System.Collections.Generic;
using Jam.Scripts.Npc;
using Jam.Scripts.Npc.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jam.Scripts
{
    public class Gameplay : MonoBehaviour
    {
        [SerializeField] private Character _characterController;
        [SerializeField] private List<NPCDefinition> _characters;
        [SerializeField] private NPCDefinition _tutorialCharacter;

        private void Awake()
        {
            _characterController.OnCharacterLeave += SetNextCharacter;
        }
        private void OnDestroy()
        {
            _characterController.OnCharacterLeave -= SetNextCharacter;
        }

        private void Start()
        {
            _characterController.SetCharacter(_tutorialCharacter);
        }
        
        private void SetNextCharacter()
        {
            _characterController.SetCharacter(_characters[Random.Range(0, _characters.Count)]);
        }
    }
}
