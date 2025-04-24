using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Ritual.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        [SerializeField] private List<Reagent> _slots;
        [Inject] private ReagentRepository _reagentRepository;
        public event Action<int> OnReagentAdded;

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
            OnReagentAdded?.Invoke(id);
        }
    }
}