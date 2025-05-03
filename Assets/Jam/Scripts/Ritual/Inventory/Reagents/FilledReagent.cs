using DG.Tweening;
using UnityEngine;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    public class FilledReagent : Reagent
    {
        [SerializeField] private Transform _fill;
        private float _maxFill;
        
        protected override void UpdateVisual(int newValue, int oldValue)
        {
            float fillAmount = (float) newValue / InventoryConfig.MaxReagentAmount * _maxFill;
            _fill.DOScaleY(fillAmount, InventoryConfig.ReagentAnimationTime).SetEase(Ease.Linear);
        }

        protected override void Awake()
        {
            base.Awake();
            _maxFill = _fill.localScale.y;
        }
    }
}