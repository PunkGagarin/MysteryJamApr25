using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.DayTime.Results;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.Npc;
using Jam.Scripts.Utils.Pause;
using Jam.Scripts.Utils.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.DayTime
{
    public class DayController : MonoBehaviour, IPointerClickHandler, IPauseHandler
    {
        [SerializeField] private SpriteRenderer _morningSprite;
        [SerializeField] private SpriteRenderer _daySprite;
        [SerializeField] private Character _characterController;
        [SerializeField] private WagonCurtains _curtains;
        [SerializeField] private CharacterResultWriter _characterResultWriter;

        [Inject] private DayConfig _dayConfig;
        [Inject] private PopupManager _popupManager;
        [Inject] private PauseService _pauseService;
        [Inject] private PlayerStatsPresenter _playerStatsPresenter;
        [Inject] private AudioService _audioService;
        
        private int _currentDay = 0;
        private int _currentClient = 0;
        
        private Color _transparentColor = new(1, 1, 1, 0);
        private Color _opaqueColor = new(1, 1, 1, 1);

        private bool _dayStarted;
        private bool _dayEnded;
        private bool _canCallNextClient;
        private Tween _currentDayTween;
        
        private bool IsLastClient =>
            _currentClient == _dayConfig.DayNpcs[_currentDay].Npcs.Count;

        private void CallNextClient()
        {
            if (!_canCallNextClient)
                return;
            
            if (!_dayStarted)
                StartDay();
            
            _canCallNextClient = false;
            _characterController.SetCharacter(_dayConfig.DayNpcs[_currentDay].Npcs[_currentClient]);
            _currentClient++;
            
            _curtains.OpenCurtains();
        }

        private void StartDay()
        {
            ResetDayValues();
            
            _currentDayTween = _morningSprite.DOColor(_transparentColor, _dayConfig.DayLength / 2)
                .OnComplete(() => _currentDayTween = _daySprite.DOColor(_transparentColor, _dayConfig.DayLength / 2)
                    .OnComplete(EndDay));
        }

        private void ResetDayValues()
        {
            _daySprite.DOColor(_opaqueColor, 0f);
            _morningSprite.DOColor(_opaqueColor, 0f);
            _dayStarted = true;
            _dayEnded = false;
            
            _characterResultWriter.ChangeDay(_currentDay);
        }

        private void EndDay()
        {
            _currentDay++;
            _canCallNextClient = false;
            _dayEnded = true;
            _dayStarted = false;
        }

        private void OnCharacterLeave()
        {
            _curtains.CloseCurtains();
        }

        private void ReactOnClosedCurtains()
        {
            if (_dayEnded || IsLastClient)
                ShowDayDetails();
            else
                AllowCallNextClient();
        }

        private void AllowCallNextClient()
        {
            _canCallNextClient = true;
        }

        private void ShowDayDetails()
        {
            var dayResultView = _popupManager.OpenPopup<DayResultView>(closeEvent: AllowCallNextClient);
            dayResultView.Initialize(_characterResultWriter, _playerStatsPresenter);
        }

        private void Awake()
        {
            _pauseService.Register(this);
            _characterController.OnCharacterLeave += OnCharacterLeave;
            _curtains.OnCurtainsClosed += ReactOnClosedCurtains;
            AllowCallNextClient();
        }

        private void OnDestroy()
        {
            _pauseService.Unregister(this);
            _characterController.OnCharacterLeave -= OnCharacterLeave;
            _curtains.OnCurtainsClosed -= ReactOnClosedCurtains;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;
            
            _audioService.PlaySound(Sounds.ropeBell.ToString());
            
            CallNextClient();
        }

        public void SetPaused(bool isPaused)
        {
            if (isPaused)
                _currentDayTween.Pause();
            else
                _currentDayTween.Play();
        }
    }
}