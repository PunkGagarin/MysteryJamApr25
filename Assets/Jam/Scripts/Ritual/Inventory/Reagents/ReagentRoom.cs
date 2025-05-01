using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    public class ReagentRoom : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private SpriteRenderer _reagentImage;
        [field: SerializeField] public ReagentDefinition ReagentInside { get; private set; }

        [Inject] private ReagentDragger _reagentDragger;
        [Inject] private InventorySystem _inventorySystem;
        
        public Vector3 Position => transform.position;

        public void SetReagent(ReagentDefinition reagent)
        {
            ReagentInside = reagent;
            _reagentImage.sprite = reagent.Visual;
        }

        public void ActivateRoom() => 
            _reagentImage.enabled = true;

        public void ReleaseReagent(bool consumeReagents)
        {
            if (!consumeReagents)
                _inventorySystem.AddReagent(ReagentInside.Id, 1);
            
            _reagentImage.enabled = false;
            ReagentInside = null;
        }

        public void AppearReagent()
        {
            _reagentImage.enabled = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;
            
            if (ReagentInside != null)
                ReleaseReagent(false);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;

            if (ReagentInside != null)
            {
                _reagentDragger.StartDrag(this, ReagentInside);
                _reagentImage.enabled = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        { }
    }
}