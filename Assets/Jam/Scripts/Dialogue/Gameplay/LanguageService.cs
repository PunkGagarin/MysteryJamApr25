using System;
using Jam.Scripts.Dialogue.Runtime.Enums;
using TMPro;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Gameplay
{
    public class LanguageService : MonoBehaviour
    {
        public event Action OnSwitchLanguage;

        [SerializeField] private LanguageType _currentLanguage;

        public LanguageType CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                _currentLanguage = value;
                OnSwitchLanguage?.Invoke();
            }
        }
    }
}