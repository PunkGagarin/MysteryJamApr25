using System;
using Jam.Scripts.Dialogue.Runtime.Enums;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Dialogue.Gameplay
{
    public class LanguageService : MonoBehaviour
    {
        public event Action OnSwitchLanguage;
        
        [Inject] private LanguageModel _languageModel;
        
        public LanguageType CurrentLanguage
        {
            get => _languageModel.Language;
            set
            {
                _languageModel.SaveLanguage(value);
                OnSwitchLanguage?.Invoke();
            }
        }
    }
}