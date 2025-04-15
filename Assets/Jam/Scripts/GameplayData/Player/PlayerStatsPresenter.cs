using System;
using Jam.Scripts.Dialogue.Runtime.Enums;

namespace Jam.Scripts.GameplayData.Player
{
    public class PlayerStatsPresenter
    {
        private PlayerStats _playerStats;

        public event Action<int> OnMoneyChanged;
        public event Action<int> OnReputationChanged;
        
        public int Money => _playerStats.Money;
        public int Reputation => _playerStats.Reputation;

        public PlayerStatsPresenter()
        {
            _playerStats = new PlayerStats();
        }

        public void AddMoney(int amount)
        {
            _playerStats.Money += amount;
            OnMoneyChanged?.Invoke(_playerStats.Money);
        }
        
        public void RemoveMoney(int amount)
        {
            _playerStats.Money -= amount;
            OnMoneyChanged?.Invoke(_playerStats.Money);
        }
        
        public void AddReputation(int amount)
        {
            _playerStats.Reputation += amount;
            OnReputationChanged?.Invoke(_playerStats.Reputation);
        }
        
        public void RemoveReputation(int amount)
        {
            _playerStats.Reputation -= amount;
            OnReputationChanged?.Invoke(_playerStats.Reputation);
        }

        public bool CheckMoney(int value, StringEventConditionType conditionType)
        {
            switch (conditionType)
            {
                case StringEventConditionType.Equals:
                    return _playerStats.Money == value;
                case StringEventConditionType.EqualsOrBigger:
                    return _playerStats.Money >= value;
                case StringEventConditionType.EqualsOrSmaller:
                    return _playerStats.Money <= value;
                case StringEventConditionType.Bigger:
                    return _playerStats.Money > value;
                case StringEventConditionType.Smaller:
                    return _playerStats.Money < value;
                case StringEventConditionType.True:
                case StringEventConditionType.False:
                default:
                    return false;
            }
        }
    }
}