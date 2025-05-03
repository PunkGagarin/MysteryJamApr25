using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.Npc.Data;
using TMPro;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.DayTime.Results
{
    public class CharacterResultView : MonoBehaviour
    {
        [SerializeField] private NpcRepository _npcRepository;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _reputation;
        [SerializeField] private TMP_Text _money;
        [SerializeField] private Color _errorColor;
        [SerializeField] private Color _successColor;
        [Inject] private Localization _localization;

        private const string DAY_RESULT_REPUTATION_KEY = "DAY_RESULT_REPUTATION_KEY";
        private const string DAY_RESULT_MONEY_KEY = "DAY_RESULT_MONEY_KEY";
        public void SetInfo(CharacterResult characterResult)
        {
            _name.text = characterResult.CharacterName;
            _reputation.text = $"{_localization.GetText(DAY_RESULT_REPUTATION_KEY)}: {characterResult.EarnReputation:+0;-0;0}";
            _reputation.color = characterResult.EarnReputation > 0 ? _successColor : _errorColor;
            _money.text = $"{_localization.GetText(DAY_RESULT_MONEY_KEY)}: {characterResult.EarnMoney:+0;-0;0}";
            _money.color = characterResult.EarnMoney > 0 ? _successColor : _errorColor;
        }
    }
}