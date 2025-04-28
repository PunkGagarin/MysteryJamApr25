using System;
using System.Collections.Generic;
using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.Dialogue.Runtime.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Dialogue.UI
{
    public class ButtonController : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public TMP_Text ButtonText { get; private set; }

        [Inject] private LanguageService _languageService;

        private List<LanguageGeneric<string>> _genericText;
        private int _index;

        public void SetText(int index, List<LanguageGeneric<string>> genericText)
        {
            _index = index;
            _genericText = genericText;
            UpdateText();
        }

        private void UpdateText() => 
            ButtonText.text = $"{_index}: {_genericText.Find(genericText => genericText.LanguageType == _languageService.CurrentLanguage).LanguageGenericType}";

        private void Awake() => 
            _languageService.OnSwitchLanguage += UpdateText;

        private void OnDestroy() => 
            _languageService.OnSwitchLanguage -= UpdateText;
    }
}