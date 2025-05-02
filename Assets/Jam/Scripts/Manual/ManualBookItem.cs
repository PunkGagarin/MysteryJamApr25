using System.Collections.Generic;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Ritual;
using Jam.Scripts.Ritual.Inventory;
using Jam.Scripts.Utils.UI;
using Jam.Scripts.VFX;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Manual
{
    public class ManualBookItem : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private PopupManager _popupManager;
        [Inject] private AudioService _audioService;
        [Inject] private InventorySystem _inventorySystem;
        [Inject] private PointerFirefly _pointerFirefly;

        private HashSet<ReagentExclusion> _reagentExclusions = new();
        private HashSet<int> _unlockedReagents = new();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject()) return;
            OpenPopup();
        }

        public bool CheckReagentExclusion(List<ReagentExclusion> excludedReagents, out ReagentExclusion reagentExclusion)
        {
            reagentExclusion = null;
            foreach (var exclusion in excludedReagents)
            {
                if (!_reagentExclusions.Contains(exclusion))
                {
                    reagentExclusion = exclusion;
                    return true;
                }
            }
            return false;
        }

        public void AddReagentExclusion(ReagentExclusion excludedReagentToAdd)
        {
            _reagentExclusions.Add(excludedReagentToAdd);
        }

        private void UnlockReagent(int reagentId) => 
            _unlockedReagents.Add(reagentId);

        private void OpenPopup()
        {
            _audioService.PlaySound(Sounds.manualOpening.ToString());
            var manualPopup = _popupManager.OpenPopup<ManualPopup>(closeEvent: () =>
            {
                if (_pointerFirefly.CurrentTarget == (int)TargetType.Book)
                    _pointerFirefly.ChangeTargetTo(TargetType.FirstReagent);
            });
            manualPopup.Initialize(_unlockedReagents, _reagentExclusions);
        }

        private void Awake()
        {
            _inventorySystem.OnReagentSeen += UnlockReagent;
        }

        private void OnDestroy()
        {
            _inventorySystem.OnReagentSeen -= UnlockReagent;
        }
    }
}