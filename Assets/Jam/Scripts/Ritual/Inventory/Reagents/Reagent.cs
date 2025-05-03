using Jam.Scripts.Audio.Domain;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    public abstract class Reagent : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ReagentDefinition _reagentDefinition;

        [Inject] private RitualController _ritualController;
        [Inject] private ReagentAnimationController _reagentAnimationController;
        [Inject] private AudioService _audioService;
        [Inject] protected InventoryConfig InventoryConfig;

        protected int CurrentAmount;
        public ReagentDefinition ReagentDefinition => _reagentDefinition;

        public void AddReagent(int amount = 1)
        {
            int oldValue = CurrentAmount;
            CurrentAmount += amount;
            if (CurrentAmount > InventoryConfig.MaxReagentAmount)
                CurrentAmount = InventoryConfig.MaxReagentAmount;
            UpdateVisual(CurrentAmount, oldValue);
        }

        protected abstract void UpdateVisual(int newValue, int oldValue);
        
        private void RemoveReagent()
        {
            if (_reagentDefinition == null)
            {
                Debug.LogError("No components in this slot!");
                return;
            }

            int oldAmount = CurrentAmount;
            CurrentAmount--;
            
            UpdateVisual(CurrentAmount, oldAmount);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_reagentDefinition == null || CurrentAmount == 0)
                return;

            if (!EventSystem.current.IsPointerOverGameObject())
                return;

            if (_ritualController.TryAddComponent(_reagentDefinition, out ReagentRoom room))
            {
                _audioService.PlaySound(_reagentDefinition.ClickClip.ToString());
                _reagentAnimationController.PlayAnimationFromInventory(_reagentDefinition, transform.position, room);
                RemoveReagent();
            }
            else
            {
                _audioService.PlaySound(Sounds.error.ToString());
            }
        }

        protected virtual void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}