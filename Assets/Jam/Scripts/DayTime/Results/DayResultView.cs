using System.Collections.Generic;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.SceneManagement;
using Jam.Scripts.Utils.Coroutine;
using Jam.Scripts.Utils.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
        [SerializeField] private TMP_Text _nextDayButtonText;
        [SerializeField] private Transform _charactersResultsContainer;
        [SerializeField] private CharacterResultView _characterResultViewPrefab;

        [Inject] private SceneLoader _sceneLoader;
        [Inject] private CoroutineHelper _coroutineHelper;
        
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
            _charactersResults = new List<CharacterResultView>();
            
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
            
            ResetCharactersResults();
            AdjustCharactersResults(dayResult.CharactersResults.Count);

            for (int i = 0; i < dayResult.CharactersResults.Count; i++)
            {
                _charactersResults[i].gameObject.SetActive(true);
                _charactersResults[i].SetInfo(dayResult.CharactersResults[i]);
            }

            if (_characterResultWriter.IsLastDay)
            {
                SetCloseEvent(LoadMainScene);
                _nextDayButtonText.text = "На главную";
            }
        }

        private void LoadMainScene() => 
            _coroutineHelper.RunCoroutine(_sceneLoader.LoadScene(SceneEnum.MainMenu));

        private void AdjustCharactersResults(int charactersResultsCount)
        {
            int resultsToCreate = charactersResultsCount - _charactersResults.Count;
            for (int i = 0; i < resultsToCreate; i++)
            {
                var characterResultView = Instantiate(_characterResultViewPrefab, _charactersResultsContainer);
                characterResultView.gameObject.SetActive(false);
                _charactersResults.Add(characterResultView);
            }
        }

        private void ResetCharactersResults()
        {
            foreach (var characterResultView in _charactersResults) 
                characterResultView.gameObject.SetActive(false);
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