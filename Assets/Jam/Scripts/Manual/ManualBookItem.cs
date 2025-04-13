using Jam.Scripts.Manual.Popup;
using Jam.Scripts.Utils.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Manual
{
    public class ManualBookItem : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private ManualPopupPresenter _presenter;
        [Inject] private PopupManager _popupManager;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject()) return;
            OpenPopup();
        }

        private void OpenPopup()
        {
            var popup = _popupManager.OpenPopup<ManualPopup>();
            popup.SetPresenter(_presenter);
        }
    }
}