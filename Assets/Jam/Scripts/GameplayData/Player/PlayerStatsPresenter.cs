using System;
using Jam.Scripts.Dialogue.Runtime.Enums;

namespace Jam.Scripts.GameplayData.Player
{
    public class PlayerStatsPresenter
    {
        public delegate void IntChanged(int newValue, int oldValue);
        private PlayerStats _playerStats;

        public event IntChanged OnMoneyChanged;
        public event IntChanged OnReputationChanged;
        
        public int Money => _playerStats.Money;
        public int Reputation => _playerStats.Reputation;
        public int TotalMoneyEarned => _playerStats.TotalMoneyEarned;
        public bool IsLastQuestComplete => _playerStats.IsLastQuestComplete;

        public PlayerStatsPresenter()
        {
            _playerStats = new PlayerStats();
        }

        public void AddMoney(int amount)
        {
            int oldValue = _playerStats.Money;
            _playerStats.Money += amount;
            _playerStats.TotalMoneyEarned += amount;
            OnMoneyChanged?.Invoke(_playerStats.Money, oldValue);
        }
        
        public void RemoveMoney(int amount)
        {
            int oldValue = _playerStats.Money;
            _playerStats.Money -= amount;
            OnMoneyChanged?.Invoke(_playerStats.Money, oldValue);
        }

        public bool CanSpend(int amount) =>
            _playerStats.Money >= amount;
        
        public void AddReputation(int amount)
        {
            int oldValue = _playerStats.Reputation;
            _playerStats.Reputation += amount;
            OnReputationChanged?.Invoke(_playerStats.Reputation, oldValue);
        }
        
        public void RemoveReputation(int amount)
        {
            int oldValue = _playerStats.Reputation;
            _playerStats.Reputation -= amount;
            OnReputationChanged?.Invoke(_playerStats.Reputation, oldValue);
        }

        public void SetLastQuestComplete() => 
            _playerStats.IsLastQuestComplete = true;

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