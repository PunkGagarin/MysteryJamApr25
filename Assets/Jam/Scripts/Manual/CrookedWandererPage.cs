using System.Collections.Generic;
using Jam.Scripts.Dialogue.Gameplay;
using TMPro;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Manual
{
    public class CrookedWandererPage : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private TMP_Text _youngText;
        [SerializeField] private TMP_Text _adultText;
        [SerializeField] private TMP_Text _oldText;
        [SerializeField] private TMP_Text _elfText;
        [SerializeField] private TMP_Text _humanText;
        [SerializeField] private TMP_Text _dwarfText;

        [Inject] private Localization _localization;
        [Inject] private LanguageService _languageService;
            
        private const string YOUNG_KEY = "PAGE_4_YOUNG";
        private const string ADULT_KEY = "PAGE_4_ADULT";
        private const string OLD_KEY = "PAGE_4_OLD";
        private const string ELF_KEY = "PAGE_4_ELF";
        private const string HUMAN_KEY = "PAGE_4_HUMAN";
        private const string DWARF_KEY = "PAGE_4_DWARF";

        private bool _dwarfCrossed, _oldYoungCrossed, _humanCrossed;

        public void SetUnlocks(List<int> crookedWandererUnlocks)
        {
            foreach (var crookedWandererUnlock in crookedWandererUnlocks) 
                UnlockState(crookedWandererUnlock);
            UpdateText();
        }
        
        private void UnlockState(int state)
        {
            switch (state)
            {
                case 1:
                    _container.gameObject.SetActive(true);
                    break;
                case 2:
                    _dwarfCrossed = true;
                    break;
                case 3:
                    _oldYoungCrossed = true;
                    break;
                case 4:
                    _humanCrossed = true;
                    break;
                default:
                    Debug.Log($"Стейт : {state} не определен в мануале на странице 4!");
                    break;
            }
        }

        private void UpdateText()
        {
            _youngText.text = _oldYoungCrossed ? $"<s>{_localization.GetText(YOUNG_KEY)}</s>" : _localization.GetText(YOUNG_KEY);
            _adultText.text = _localization.GetText(ADULT_KEY);
            _oldText.text = _oldYoungCrossed ? $"<s>{_localization.GetText(OLD_KEY)}</s>" : _localization.GetText(OLD_KEY);
            _elfText.text = _localization.GetText(ELF_KEY);
            _humanText.text = _humanCrossed ? $"<s>{_localization.GetText(HUMAN_KEY)}</s>" : _localization.GetText(HUMAN_KEY);
            _dwarfText.text = _dwarfCrossed ? $"<s>{_localization.GetText(DWARF_KEY)}</s>" : _localization.GetText(DWARF_KEY);
        }

        private void Awake()
        {
            _container.gameObject.SetActive(false);
            _languageService.OnSwitchLanguage += UpdateText;
        }

        private void OnDestroy()
        {
            _languageService.OnSwitchLanguage -= UpdateText;
        }
    }
}