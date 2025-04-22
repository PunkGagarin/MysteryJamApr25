using System.Collections.Generic;

namespace Jam.Scripts.DayTime.Results
{
    public class DayInfo
    {
        public int DayNumber;
        public List<CharacterResult> CharactersResults = new();
        public int TotalEarnMoney;
    }
}