using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Ritual.Inventory.Reagents;
using Jam.Scripts.Ritual.Tools;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Ritual.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        [SerializeField] private List<Reagent> _slots;
        [Inject] private ReagentRepository _reagentRepository;
        [Inject] private InventoryConfig _inventoryConfig;
        [Inject] private ToolController _toolController;
        public event Action<int> OnReagentSeen;

        public void BuyReagent(int id) => 
            AddReagent(id, _inventoryConfig.MaxReagentAmount);

        public void AddReagent(int id, int amount)
        {
            if (_slots.All(slot => !slot.IsEmpty))
            {
                Debug.LogError("No free slots");
                return;
            }
            
            var reagentDefinition = _reagentRepository.GetDefinition(id);

            var slot = _slots[reagentDefinition.Id - 1];
            slot.SetReagent(reagentDefinition);
            slot.AddReagent(amount);

            SeenReagent(id);
        }

        public void SeenReagent(int id) => 
            OnReagentSeen?.Invoke(id);

        public void BuyTool(ToolDefinition unlockedTool)
        {
            _toolController.BuyTool(unlockedTool);
        }
    }
}