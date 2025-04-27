using System;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Runtime.Events.SO
{
    [CreateAssetMenu(menuName = "Game Resources/Dialogue/Events/Penalty Event")]
    public class PenaltyEvent : DialogueEventSO
    {
        [SerializeField] private int _moneyPenalty = 0;
        [SerializeField] private int _reputationPenalty = 0;
        public event Action<Penalty> OnPenalty;
        
        public override void RunEvent()
        {
            Penalty penalty = new Penalty
            {
                MoneyPenalty = _moneyPenalty,
                ReputationPenalty = _reputationPenalty
            };
            OnPenalty?.Invoke(penalty);
        }

        public class Penalty
        {
            public int MoneyPenalty { get; set; }
            public int ReputationPenalty { get; set; }
        }
    }
}