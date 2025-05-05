using System.Collections.Generic;
using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.End;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.SceneManagement;
using Jam.Scripts.Shop;
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
        [SerializeField] private TMP_Text _earnedMoney;
        [SerializeField] private TMP_Text _totalReputation;
        [SerializeField] private TMP_Text _totalMoney;
        [SerializeField] private Button _nextDayButton;
        [SerializeField] private ToLocalize _nextDayButtonText;
        [SerializeField] private Transform _charactersResultsContainer;
        [SerializeField] private CharacterResultView _characterResultViewPrefab;
        [SerializeField] private ShopView _shopView;
        [SerializeField] private Color _errorColor;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _successColor;

        [Inject] private LanguageService _languageService;
        [Inject] private Localization _localization;
        [Inject] private AudioService _audioService;
        [Inject] private CharacterResultFactory _characterResultFactory;
        
        private EndScreen _endScreen;
        private List<CharacterResultView> _charactersResults;
        private CharacterResultWriter _characterResultWriter;
        private ShopSystem _shopSystem;

        private PlayerStatsPresenter _playerStats;
        private bool _isInitialized;
        private bool _isShowShop;

        private const string DAY_LOCALIZE_KEY = "DAY_RESULT_NUM";
        private const string TO_MAIN_KEY = "TO_MAIN_KEY";

        public override void Open(bool withPause)
        {
            base.Open(withPause);
            if (_isInitialized)
                ShowResult();
        }

        public void Initialize(CharacterResultWriter characterResultWriter,
            PlayerStatsPresenter playerStatsPresenter,
            ShopSystem shopSystem, EndScreen endScreen)
        {
            if (_isInitialized)
                return;

            _isInitialized = true;
            _characterResultWriter = characterResultWriter;
            _playerStats = playerStatsPresenter;
            _charactersResults = new List<CharacterResultView>();
            _shopSystem = shopSystem;
            _shopSystem.SetShopView(_shopView);
            _shopSystem.CantBuy += ShowCantBuyAnimation;
            _playerStats.OnMoneyChanged += UpdateTotalMoney;
            _endScreen = endScreen;

            ShowResult();
        }

        private void ShowResult()
        {
            _shopSystem.ShowShop();
            var dayResult = _characterResultWriter.DayInfo;
            _dayNumber.text = $"{_localization.GetText(DAY_LOCALIZE_KEY)} {dayResult.DayNumber + 1}";
            _earnedMoney.text = $"{dayResult.TotalEarnMoney:+0;-0;0}";
            _earnedMoney.color = dayResult.TotalEarnMoney > 0 ?_successColor : _errorColor;
            UpdateTotalMoney(_playerStats.Money, 0);
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
                SetCloseEvent(() =>
                {
                    _endScreen.gameObject.SetActive(true);
                    _endScreen.Show();
                });
                _nextDayButtonText.SetKey(TO_MAIN_KEY);
            }
        }

        private void UpdateTotalMoney(int newvalue, int oldvalue)
        {
            
            DOTween.To(() => oldvalue, x => {
                oldvalue = x;
                _totalMoney.text = oldvalue.ToString("N0");
            }, newvalue, 1f).SetEase(Ease.OutQuart);
        }

        private void AdjustCharactersResults(int charactersResultsCount)
        {
            int resultsToCreate = charactersResultsCount - _charactersResults.Count;
            for (int i = 0; i < resultsToCreate; i++)
            {
                var characterResultView = _characterResultFactory.Create(_characterResultViewPrefab, _charactersResultsContainer);
                characterResultView.gameObject.SetActive(false);
                _charactersResults.Add(characterResultView);
            }
        }

        private void ResetCharactersResults()
        {
            foreach (var characterResultView in _charactersResults)
                characterResultView.gameObject.SetActive(false);
        }

        private void ShowCantBuyAnimation()
        {
            _audioService.PlaySound(Sounds.error);
            _totalMoney.transform.DOKill();
            _totalMoney.transform.DOShakePosition(.2f, 5f);
            _totalMoney
                .DOColor(_errorColor, 0.5f)
                .OnComplete(() => _totalMoney.DOColor(_normalColor, 0.2f));
        }

        private void OnNextDayButtonClicked()
        {
            _audioService.PlaySound(Sounds.buttonClick);
            Close();
        }

        private void Awake()
        {
            _nextDayButton.onClick.AddListener(OnNextDayButtonClicked);
        }

        private void OnDestroy()
        {
            _nextDayButton.onClick.RemoveListener(OnNextDayButtonClicked);
            _playerStats.OnMoneyChanged -= UpdateTotalMoney;
            _shopSystem.CantBuy -= ShowCantBuyAnimation;
        }
    }
}