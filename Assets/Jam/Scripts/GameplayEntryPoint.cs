using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Ritual.Inventory;
using UnityEngine;
using Zenject;

namespace Jam.Scripts
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [Inject] private AudioService _audioService;
        [Inject] private InventoryConfig _inventoryConfig;
        [Inject] private InventorySystem _inventorySystem;
        
        private void Start()
        {
            _audioService.PlayMusic(Sounds.gameplayBgm.ToString(), true);
            foreach (var reagent in _inventoryConfig.StartReagents) 
                _inventorySystem.AddReagent(reagent.Id, _inventoryConfig.MaxReagentAmount);
        }
    }
}
