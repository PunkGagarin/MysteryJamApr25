using System.Collections.Generic;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.Utils.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.DayTime.Results
{
    public class DayResultView : Popup
    {
        [SerializeField] private TMP_Text _dayNumber; 
        [SerializeField] private TMP_Text _thoughts;
        [SerializeField] private TMP_Text _earnedMoney;
        [SerializeField] private TMP_Text _totalReputation;
        [SerializeField] private TMP_Text _totalMoney;
        [SerializeField] private Button _nextDayButton;
        [SerializeField] private Transform _charactersResultsContainer;
        [SerializeField] private CharacterResultView _characterResultViewPrefab;

        private List<CharacterResultView> _charactersResults;
        private CharacterResultWriter _characterResultWriter;

        private PlayerStatsPresenter _playerStats;
        private bool _isInitialized;
        
        public override void Open(bool withPause)
        {
            base.Open(withPause);
            if (_isInitialized)
                ShowResult();
        }

        public void Initialize(CharacterResultWriter characterResultWriter, PlayerStatsPresenter playerStatsPresenter)
        {
            if (_isInitialized)
                return;
            
            _isInitialized = true;
            _characterResultWriter = characterResultWriter;
            _playerStats = playerStatsPresenter;
            ShowResult();
        }

        private void ShowResult()
        {
            var dayResult = _characterResultWriter.DayInfo;

            _dayNumber.text = dayResult.DayNumber.ToString(); 
            _thoughts.text = "тут должны были быть мысли но у меня их нет (";
            _earnedMoney.text = dayResult.TotalEarnMoney.ToString();
            _totalMoney.text = _playerStats.Money.ToString();
            _totalReputation.text = _playerStats.Reputation.ToString();
        }

        private void Awake()
        {
            _nextDayButton.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            _nextDayButton.onClick.RemoveListener(Close);
        }
    }
}