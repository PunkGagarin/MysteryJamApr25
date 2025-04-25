using Jam.Scripts.Audio.Domain;
using Jam.Scripts.DayTime.Results;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.Npc;
using Jam.Scripts.Npc.Data;
using Jam.Scripts.Utils.Pause;
using Jam.Scripts.Utils.UI;
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
        
        private int _currentDay = 0;
        private int _currentClient = 0;
        private bool _canCallNextClient;
        
        private bool IsLastClient =>
            _currentClient == _dayConfig.DayNpcs[_currentDay].Npcs.Count;

        public bool IsLastDay =>
            _currentDay >= _dayConfig.DayNpcs.Count;

        private void CallNextClient()
        {
            if (!_canCallNextClient)
                return;
            
            _canCallNextClient = false;
            NPCDefinition npcDefinition = GetNpcFromConfig();
            _characterController.SetCharacter(npcDefinition);
            _currentClient++;
            
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
            _currentClient = 0;
            _canCallNextClient = false;
            
            _currentDay++;
            ShowDayDetails();
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
            var dayResultView = _popupManager.OpenPopup<DayResultView>(closeEvent: ResetDayValues);
            dayResultView.Initialize(_characterResultWriter, _playerStatsPresenter);
        }

        private void Awake()
        {
            _characterController.OnCharacterLeave += OnCharacterLeave;
            _curtains.OnCurtainsClosed += ReactOnClosedCurtains;
            ResetDayValues();
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