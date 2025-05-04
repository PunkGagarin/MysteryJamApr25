using Jam.Scripts.Audio.Domain;
using Jam.Scripts.DayTime.Results;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.Npc;
using Jam.Scripts.Npc.Data;
using Jam.Scripts.Quests;
using Jam.Scripts.Ritual.Desk;
using Jam.Scripts.Ritual.Tools;
using Jam.Scripts.Shop;
using Jam.Scripts.UI;
using Jam.Scripts.Utils.Pause;
using Jam.Scripts.Utils.UI;
using Jam.Scripts.VFX;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.DayTime
{
    public class DayController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private WagonCurtains _curtains;
        [SerializeField] private CharacterResultWriter _characterResultWriter;
        [SerializeField] private RandomNpcController _randomNpcController;

        [Inject] private Character _characterController;
        [Inject] private DayConfig _dayConfig;
        [Inject] private PopupManager _popupManager;
        [Inject] private PauseService _pauseService;
        [Inject] private PlayerStatsPresenter _playerStatsPresenter;
        [Inject] private AudioService _audioService;
        [Inject] private ShopSystem _shopSystem;
        [Inject] private ToolController _toolController;
        [Inject] private PointerFirefly _pointerFirefly;
        [Inject] private GameplayOverlayUI _gameplayOverlayUI;
        [Inject] private QuestPresenter _questPresenter;
        [Inject] private Memory _memory;

        private int _currentDay = 0;
        private int _currentClient = 0;
        private bool _canCallNextClient;

        private bool IsLastClient =>
            _currentClient == _dayConfig.DayNpcs[_currentDay].Npcs.Count;

        public bool IsLastDay =>
            _currentDay >= _dayConfig.DayNpcs.Count;

        public bool IsFirstDay =>
            _currentDay == 1;

        private void CallNextClient()
        {
            if (!_canCallNextClient)
                return;

            _canCallNextClient = false;
            NPCDefinition npcDefinition = GetNpcFromConfig();
            Debug.Log($"Npc arrived : {npcDefinition.name}");
            _characterController.SetCharacter(npcDefinition);
            _currentClient++;

            if (!_questPresenter.HaveAnyCompletedQuest())
                _pointerFirefly.ChangeTargetTo(TargetType.Character);

            _curtains.OpenCurtains();
        }

        private NPCDefinition GetNpcFromConfig()
        {
            var currentDayNpc = _dayConfig.DayNpcs[_currentDay].Npcs[_currentClient];
            return currentDayNpc.IsRandomNpc ? _randomNpcController.GetNpc() : currentDayNpc.Npc;
        }

        private void ResetDayValues()
        {
            AllowCallNextClient();

            _characterResultWriter.ChangeDay(_currentDay);
        }

        private void EndDay()
        {
            _currentDay++;
            SetMemory();
            _currentClient = 0;
            _canCallNextClient = false;
            ShowDayDetails();
        }

        private void SetMemory()
        {
            var memoryConfig = _dayConfig.GetMemoryConfig(_currentDay);
            if (memoryConfig != null)
                _memory.SetMemoryConfig(memoryConfig);
        }

        private void OnCharacterLeave() =>
            _curtains.CloseCurtains();

        private void ReactOnClosedCurtains()
        {
            if (IsLastClient)
                EndDay();
            else
                AllowCallNextClient();
        }

        private void AllowCallNextClient()
        {
            _canCallNextClient = true;
        }

        private void ShowDayDetails()
        {
            var dayResultView = _popupManager.OpenPopup<DayResultView>(OnOpenDayResultView, OnCloseDayResultView);
            dayResultView.Initialize(_characterResultWriter, _playerStatsPresenter, _shopSystem);
        }

        private void OnOpenDayResultView() => 
            _gameplayOverlayUI.gameObject.SetActive(false);

        private void OnCloseDayResultView()
        {
            _gameplayOverlayUI.gameObject.SetActive(true);
            ResetDayValues();
        }

        private void Awake()
        {
            _characterController.OnCharacterLeave += OnCharacterLeave;
            _curtains.OnCurtainsClosed += ReactOnClosedCurtains;
            ResetDayValues();
            SetMemory();
        }

        private void OnDestroy()
        {
            _characterController.OnCharacterLeave -= OnCharacterLeave;
            _curtains.OnCurtainsClosed -= ReactOnClosedCurtains;
            _popupManager.ResetPopup<DayResultView>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;

            _audioService.PlaySound(Sounds.ropeBell.ToString());

            CallNextClient();
        }
    }
}