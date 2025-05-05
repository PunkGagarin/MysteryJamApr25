using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Ritual.Tools
{
    public class MagnifierRitualTool : RitualTool, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer _rightStateVisual;

        [Inject] private RitualController _ritualController;
        [Inject] private AudioService _audioService;

        private void CheckRitual()
        {
            if (Charges == 0 || !IsUnlocked || !_ritualController.CanCheckByMagnifier)
            {
                _audioService.PlaySound(Sounds.error);
                return;
            }
            
            _audioService.PlaySound(Sounds.buttonClick);            

            Charges--;
            ShowUpdateCharges();

            bool foundExcluded = _ritualController.CheckForExcludedReagents();

            BlinkVisual(foundExcluded ? Visual : _rightStateVisual);
        }

        private void BlinkVisual(SpriteRenderer visual)
        {
            visual.DOColor(Color.white, 0.5f)
                .SetLoops(6, LoopType.Yoyo)
                .SetEase(Ease.Linear)
                .OnComplete(() => Visual.color = Color.clear);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;

            CheckRitual();
        }
    }
}