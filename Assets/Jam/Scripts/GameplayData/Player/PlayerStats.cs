namespace Jam.Scripts.GameplayData.Player
{
    public class PlayerStats
    {
        public int Money { get; set; }
        public int Reputation { get; set; }
        public int TotalMoneyEarned { get; set; }
        public bool IsLastQuestComplete { get; set; } = false;
    }
}
