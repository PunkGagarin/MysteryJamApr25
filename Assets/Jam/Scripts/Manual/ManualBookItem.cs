using Jam.Scripts.Audio.Domain;
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

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject()) return;
            OpenPopup();
        }

        private void OpenPopup()
        {
            _audioService.PlaySound(Sounds.manualOpening.ToString());
            _popupManager.OpenPopup<ManualPopup>();
        }
    }
}