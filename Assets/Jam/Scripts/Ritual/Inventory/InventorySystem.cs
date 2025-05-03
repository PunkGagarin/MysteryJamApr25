using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Ritual.Inventory.Reagents;
using Jam.Scripts.Ritual.Tools;
using Jam.Scripts.Shop;
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
        [Inject] private ShopSystem _shopSystem;
        public event Action<int> OnReagentSeen;

        public void BuyReagent(int id) => 
            AddReagent(id, _inventoryConfig.MaxReagentAmount);

        public void AddReagent(int id, int amount)
        {
            var reagentDefinition = _reagentRepository.GetDefinition(id);

            var slot = _slots.FirstOrDefault(slot => slot != null && slot.ReagentDefinition == reagentDefinition);
            if (slot == null)
            {
                Debug.LogError($"Trying to add reagent with id:{id}, but it isn't in a inventorySystem!");
                return;
            }

            slot.gameObject.SetActive(true);
            slot.AddReagent(amount);

            SeenReagent(id);
        }

        private void SeenReagent(int id) => 
            OnReagentSeen?.Invoke(id);

        public void BuyTool(ToolDefinition unlockedTool) => 
            _toolController.BuyTool(unlockedTool);

        private void Awake() => 
            _shopSystem.ItemAppear += SeenReagent;

        private void OnDestroy() =>
            _shopSystem.ItemAppear -= SeenReagent;

        public List<ToolDefinition> GetUnlockedTools() =>
            _toolController.GetUnlockedTools();
    }
}