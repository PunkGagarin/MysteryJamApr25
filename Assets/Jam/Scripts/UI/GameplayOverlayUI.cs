using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.MainMenuPopups;
using Jam.Scripts.Utils.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.UI
{
    public class GameplayOverlayUI : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private GameObject _content;
        [SerializeField] private TMP_Text _moneyAmount;
        
        [Inject] private PlayerStatsPresenter _playerStatsPresenter;
        [Inject] private PopupManager _popupManager;
        [Inject] private AudioService _audioService;
    
    
        private void Awake()
        {
            _pauseButton.onClick.AddListener(OnPauseClick);
            _playerStatsPresenter.OnMoneyChanged += OnMoneyChanged;
        }

        private void OnMoneyChanged(int newvalue, int oldvalue)
        {
            DOTween.To(() => oldvalue, x => {
                oldvalue = x;
                _moneyAmount.text = oldvalue.ToString("N0");
            }, newvalue, 1f).SetEase(Ease.OutQuart);
            //_audioService.PlaySound(Sounds.moneyChanged); todo
        }

        private void Start()
        {
            _moneyAmount.text = 0.ToString();
        }

        private void OnDestroy()
        {
            _pauseButton.onClick.RemoveListener(OnPauseClick);
            _playerStatsPresenter.OnMoneyChanged -= OnMoneyChanged;
        }

        private void OnPauseClick()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            _popupManager.OpenPopup<PausePopup>(OnOpenEvent, OnPopupClose, true);
        }

        private void OnOpenEvent() => _content.SetActive(false);

        private void OnPopupClose() => _content.SetActive(true);
    }
}