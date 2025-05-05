using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Ritual;
using Jam.Scripts.Ritual.Inventory;
using Jam.Scripts.Ritual.Tools;
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
        private List<int> _crookedWandererUnlocks = new();
        private bool _artUnlocked = false;

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
            var unlockedTools = _inventorySystem.GetUnlockedTools();
            bool isMirrorUnlocked = unlockedTools.Any(tool => tool.Id == 0);
            bool isMagnifierUnlocked = unlockedTools.Any(tool => tool.Id == 1);
            manualPopup.Initialize(_unlockedReagents, _reagentExclusions, _crookedWandererUnlocks, _artUnlocked, isMirrorUnlocked, isMagnifierUnlocked);
        }

        private void Awake()
        {
            _inventorySystem.OnReagentSeen += UnlockReagent;
        }

        private void OnDestroy()
        {
            _inventorySystem.OnReagentSeen -= UnlockReagent;
        }

        public void UpdatePage(int pageIndex, int pageState)
        {
            if (pageIndex == 4) 
                _crookedWandererUnlocks.Add(pageState);
            if (pageIndex == 5)
                _artUnlocked = true;
            _audioService.PlaySound(Sounds.foundConflict);
        }
    }
}