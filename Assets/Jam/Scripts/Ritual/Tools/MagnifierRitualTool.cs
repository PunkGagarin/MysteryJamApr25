using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Ritual.Tools
{
    public class MagnifierRitualTool : RitualTool, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer _checkStateImage;
        [SerializeField] private Color _foundExclusionColor;
        [SerializeField] private Color _clearColor;
        
        [Inject] private RitualController _ritualController;

        private void CheckRitual()
        {
            if (Charges == 0 || !IsUnlocked || !_ritualController.CanCheckByMagnifier)
                return;

            Charges--;
            ShowUpdateCharges();

            Color color = _ritualController.CheckForExcludedReagents() ? _foundExclusionColor : _clearColor;
            Color transparentColor = color;
            transparentColor.a = 0;
            _checkStateImage.color = transparentColor;
            _checkStateImage.DOColor(color, 1f).SetLoops(1, LoopType.Yoyo);
        }

        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;
            
            CheckRitual();
        }
    }
}