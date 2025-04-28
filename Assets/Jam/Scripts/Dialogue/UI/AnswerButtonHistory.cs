using System.Collections.Generic;
using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.Dialogue.Runtime.SO;
using TMPro;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Dialogue.UI
{
    public class AnswerButtonHistory : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        [Inject] private LanguageService _languageService;
        
        private List<LanguageGeneric<string>> _genericText;

        public void SetText(List<LanguageGeneric<string>> genericText)
        {
            _genericText = genericText;
            UpdateLanguageText();
        }

        private void UpdateLanguageText() => 
            _text.text = _genericText.Find(genericText => genericText.LanguageType == _languageService.CurrentLanguage).LanguageGenericType;

        private void Awake() => 
            _languageService.OnSwitchLanguage += UpdateLanguageText;

        private void OnDestroy() => 
            _languageService.OnSwitchLanguage -= UpdateLanguageText;
    }
}