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
        private HashSet<Popup> _popups = new HashSet<Popup>();
        
        public void OpenPopup<T>(Action closeEvent = null, bool withPause = false) where T : Popup
        {
            foreach (T popup in _popups.OfType<T>())
            {
                popup.SetCloseEvent(closeEvent);
                popup.Open(withPause);
                return;
            }

            AssignNewPopup<T>(closeEvent, withPause);
        }
        
        private void AssignNewPopup<T>(Action closeEvent, bool withPause) where T : Popup
        {
            T newPopup = _factory.CreatePopup<T>();
            newPopup.transform.SetParent(transform, false);
            newPopup.SetCloseEvent(closeEvent);
            newPopup.Open(withPause);
            _popups.Add(newPopup);
        }
    }
}
