using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Ritual.Tools
{
    public abstract class RitualTool : MonoBehaviour
    {
        [SerializeField] private protected SpriteRenderer Visual;
        [SerializeField] private Image _fill;
        [SerializeField] private float _fillAnimationTime;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ToolDefinition _definition;
        
        protected int Charges;
        
        public ToolDefinition Definition => _definition;
        public bool IsUnlocked { get; set; }
        
        public void ResetCharges()
        {
            Charges = _definition.Charges;
            ShowUpdateCharges();
        }
        
        protected void ShowUpdateCharges() => 
            _fill.DOFillAmount(Charges/(float)_definition.Charges, _fillAnimationTime).SetEase(Ease.Linear);

        private void Awake()
        {
            _canvas.worldCamera = UnityEngine.Camera.main;
            Charges = 0;
        }
    }
}