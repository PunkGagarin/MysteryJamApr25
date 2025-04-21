using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    public class ReagentRoom : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _reagentImage;
        [field: SerializeField] public ReagentDefinition ReagentInside { get; private set; }

        public event Action OnRoomChanged;
        
        private InventorySystem _inventorySystem;
        public bool IsFree => ReagentInside == null;
        public Vector3 Position => transform.position;

        public void Initialize(InventorySystem inventorySystem) => 
            _inventorySystem = inventorySystem;
        
        public void SetReagent(ReagentDefinition reagent)
        {
            ReagentInside = reagent;
            _reagentImage.sprite = reagent.Visual;
        }

        public void ActivateRoom()
        {
            _reagentImage.enabled = true;
            OnRoomChanged?.Invoke();
        }
        public void ReleaseReagent(bool consumeReagents)
        {
            if (!consumeReagents)
                _inventorySystem.AddReagent(ReagentInside.Id, 1);
            
            _reagentImage.enabled = false;
            ReagentInside = null;
            OnRoomChanged?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;
            
            if (!IsFree)
                ReleaseReagent(false);
        }
    }
}