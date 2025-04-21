using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    public class Reagent : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _fill;
        [Inject] private RitualController _ritualController;
        [Inject] private ReagentAnimationController _reagentAnimationController;
        [Inject] private AudioService _audioService;
        [Inject] private InventoryConfig _inventoryConfig;
        
        private ReagentDefinition _reagentDefinition;
        [SerializeField] private int _amount;
        public bool IsEmpty => _reagentDefinition == null;

        public void SetReagent(ReagentDefinition reagentDefinition)
        {
            _reagentDefinition = reagentDefinition;

            _spriteRenderer.gameObject.SetActive(true);
            _spriteRenderer.sprite = reagentDefinition.Visual;

            _name.gameObject.SetActive(true);
            _name.text = reagentDefinition.Name;
        }

        public void AddReagent(int amount = 1)
        {
            _amount += amount;
            if (_amount > _inventoryConfig.MaxReagentAmount)
                _amount = _inventoryConfig.MaxReagentAmount;
            FillAnimation();
        }

        private void FillAnimation() =>
            _fill.DOFillAmount((float)_amount / _inventoryConfig.MaxReagentAmount,
                _inventoryConfig.ReagentAnimationTime).SetEase(Ease.Linear);

        private void RemoveReagent()
        {
            if (_reagentDefinition == null)
            {
                Debug.LogError("No components in this slot!");
                return;
            }

            _amount--;
            if (_amount == 0)
            {
                _fill.DOKill();
                _reagentDefinition = null;
                _spriteRenderer.gameObject.SetActive(false);
                _name.gameObject.SetActive(false);
            }
            else
            {
                FillAnimation();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_reagentDefinition == null)
                return;

            if (!EventSystem.current.IsPointerOverGameObject())
                return;

            if (_ritualController.TryAddComponent(_reagentDefinition, out ReagentRoom room))
            {
                _audioService.PlaySound(_reagentDefinition.ClickClip.ToString());
                _reagentAnimationController.PlayAnimation(_reagentDefinition, transform.position, room);
                RemoveReagent();
            }
            else
            {
                _audioService.PlaySound(Sounds.error.ToString());
            }
        }

        private void Awake()
        {
            _spriteRenderer.gameObject.SetActive(false);
            _name.gameObject.SetActive(false);
        }
    }
}