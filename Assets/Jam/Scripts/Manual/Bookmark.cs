using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Jam.Scripts.Manual
{
    public class Bookmark : MonoBehaviour, IPointerClickHandler
    {
        public int PageIndex;

        public UnityEvent OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;

            OnClick?.Invoke();
        }
    }
}