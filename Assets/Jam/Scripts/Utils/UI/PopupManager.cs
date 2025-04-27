using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Factories;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Utils.UI
{
    public class PopupManager : MonoBehaviour
    {
        [Inject] private PopupFactory _factory;
        private HashSet<Popup> _popups = new();
        
        public Popup CurrentPopup { get; private set; }
        
        public T OpenPopup<T>(Action closeEvent = null, bool withPause = false) where T : Popup
        {
            closeEvent += () => CurrentPopup = null;
            foreach (T popup in _popups.OfType<T>())
            {
                popup.Open(withPause);
                popup.transform.SetAsLastSibling();
                popup.SetCloseEvent(closeEvent);
                CurrentPopup = popup;
                return popup;
            }

            CurrentPopup = AssignNewPopup<T>(closeEvent, withPause); 
            return (T) CurrentPopup;
        }

        public void ResetPopup<T>() where T : Popup
        {
            var popupToRemove = _popups.FirstOrDefault(popup => popup.GetType() == typeof(T));
            if (popupToRemove != null)
                _popups.Remove(popupToRemove);
        }
        
        private T AssignNewPopup<T>(Action closeEvent, bool withPause) where T : Popup
        {
            T newPopup = _factory.CreatePopup<T>();
            newPopup.transform.SetParent(transform, false);
            newPopup.Open(withPause);
            newPopup.SetCloseEvent(closeEvent);
            _popups.Add(newPopup);
            return newPopup; 
        }
        
    }
}
