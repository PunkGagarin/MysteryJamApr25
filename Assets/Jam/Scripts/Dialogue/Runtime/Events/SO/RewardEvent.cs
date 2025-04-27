using System;
using System.Collections.Generic;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Runtime.Events.SO
{
    [CreateAssetMenu(menuName = "Game Resources/Dialogue/Events/Reward Event")]
    public class RewardEvent : DialogueEventSO
    {
        [SerializeField] private int _moneyReward;
        [SerializeField] private int _reputationReward;
        [SerializeField] private List<ReagentDefinition> _reagentsReward;

        public event Action<Reward> OnReward;
        
        public override void RunEvent()
        {
            Reward eventReward = new Reward
            {
                MoneyReward = _moneyReward,
                ReputationReward = _reputationReward,
                ReagentsReward = _reagentsReward
            };
            OnReward?.Invoke(eventReward);
        }
    }

    public class Reward
    {
        public int MoneyReward { get; set; }
        public int ReputationReward { get; set; }
        public List<ReagentDefinition> ReagentsReward { get; set; }
    }
}