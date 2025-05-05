using TMPro;
using UnityEngine;

namespace Jam.Scripts.Ritual.Tools
{
    public abstract class RitualTool : MonoBehaviour
    {
        [SerializeField] private protected SpriteRenderer Visual;
        [SerializeField] private TMP_Text _fill;
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
            _fill.text = $"{Charges} / {_definition.Charges}";
        
        private void Awake()
        {
            _canvas.worldCamera = UnityEngine.Camera.main;
            Charges = 0;
        }
    }
}