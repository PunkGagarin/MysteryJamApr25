using System;

namespace Jam.Scripts.GameplayData.Player
{
    public class PlayerStats
    {
        public int Money { get; private set; }
        public int Reputation { get; private set; }
        
        public event Action<int> MoneyChanged; 
        public event Action<int> ReputationChanged; 
        
        public void AddMoney(int amount)
        {
            Money += amount;
            MoneyChanged?.Invoke(Money);
        }

        public void RemoveMoney(int amount)
        {
            Money -= amount;
            MoneyChanged?.Invoke(Money);
        }
        
        public void AddReputation(int amount)
        {
            Reputation += amount;
            ReputationChanged?.Invoke(Reputation);
        }

        public void RemoveReputation(int amount)
        {
            Reputation -= amount;
            ReputationChanged?.Invoke(Reputation);
        }
    }
}
