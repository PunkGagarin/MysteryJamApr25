using System.Collections.Generic;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Ritual;
using Jam.Scripts.Utils.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Manual
{
    public class ManualBookItem : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private PopupManager _popupManager;
        [Inject] private AudioService _audioService;
        private List<ReagentExclusion> _reagentExclusions = new();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject()) return;
            OpenPopup();
        }

        private void OpenPopup()
        {
            _audioService.PlaySound(Sounds.manualOpening.ToString());
            var manualPopup = _popupManager.OpenPopup<ManualPopup>();
            manualPopup.Initialize(_reagentExclusions);
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
    }
}