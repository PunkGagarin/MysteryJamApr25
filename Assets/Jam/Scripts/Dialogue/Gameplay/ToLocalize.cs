using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Dialogue.Gameplay
{
    [RequireComponent(typeof(Graphic))]
    public class ToLocalize : MonoBehaviour
    {
        [SerializeField] private string _key;
        [SerializeField] private TMP_FontAsset _fontAsset; 
        private RectTransform _rectTransform;
        private TextMeshProUGUI _tmpText;

        [Inject] private LanguageService _languageService;
        [Inject] private Localization _localization;

        public void SetKey(string keyValue)
        {
            _key = keyValue;
            SwitchText();
        }
         
        private void Start()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            _tmpText = GetComponent<TextMeshProUGUI>();
            _languageService.OnSwitchLanguage += SwitchText;
            SwitchText();
        }

        private void OnDestroy() => 
            _languageService.OnSwitchLanguage -= SwitchText;

        private void SwitchText()
        {
            _tmpText.text = _localization.GetText(_key).Replace("\\n", "\n");

            if (_fontAsset != null) _tmpText.font = _fontAsset;

            if (_tmpText.gameObject.transform.childCount > 0)
            {
                var children = _tmpText.gameObject.GetComponentsInChildren<TMP_SubMeshUI>();
                if (children != null)
                {
                    foreach (var child in children)
                        Destroy(child.gameObject);
                }
            }
        }
    }
}