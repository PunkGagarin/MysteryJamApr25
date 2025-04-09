using Jam.Scripts.Dialogue.Runtime.Enums;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Gameplay
{
    public class LanguageController : MonoBehaviour
    {
        [SerializeField] private LanguageType _currentLanguage;

        public LanguageType CurrentLanguage
        {
            get => _currentLanguage;
            set => _currentLanguage = value;
        }
    }
}