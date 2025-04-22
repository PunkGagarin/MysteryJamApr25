using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.Npc;
using Jam.Scripts.Npc.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.DayTime.Results
{
    public class CharacterResultWriter : MonoBehaviour
    {
        [SerializeField] private NpcRepository _charactersRepository;
        [SerializeField] private Character _characterController;
        [Inject] private PlayerStatsPresenter _playerStatsPresenter;
        [Inject] private DayController _dayController;

        public DayInfo DayInfo { get; private set; }
        public bool IsLastDay => _dayController.IsLastDay;
        
        private CharacterResult _currentCharacterResult;
        
        public void ChangeDay(int currentDay)
        {
            DayInfo = new DayInfo
            {
                TotalEarnMoney = 0,
                DayNumber = currentDay
            };
        }

        private void FinalizeWrite() => 
            _currentCharacterResult = null;

        private void UpdateCharacter(int characterId)
        {
            CharacterResult characterResult = new CharacterResult
            {
                CharacterId = characterId,
                EarnReputation = 0,
                EarnMoney = 0
            };
            _currentCharacterResult = characterResult;
            DayInfo.CharactersResults.Add(characterResult);
        }

        private void CountReputation(int value)
        {
            if (_currentCharacterResult != null)
                _currentCharacterResult.EarnReputation += value;
        }
        
        private void CountMoney(int value)
        {
            if (_currentCharacterResult != null)
                _currentCharacterResult.EarnMoney += value;

            DayInfo.TotalEarnMoney += value;
        }
        
        private void Awake()
        {
            _characterController.OnCharacterArrived += UpdateCharacter;
            _characterController.OnCharacterLeave += FinalizeWrite;
            _playerStatsPresenter.OnMoneyChanged += CountMoney;
            _playerStatsPresenter.OnReputationChanged += CountReputation;
        }

        private void OnDestroy()
        {
            _characterController.OnCharacterArrived -= UpdateCharacter;
            _characterController.OnCharacterLeave -= FinalizeWrite;
            _playerStatsPresenter.OnMoneyChanged -= CountMoney;
            _playerStatsPresenter.OnReputationChanged -= CountReputation;
        }
    }
}