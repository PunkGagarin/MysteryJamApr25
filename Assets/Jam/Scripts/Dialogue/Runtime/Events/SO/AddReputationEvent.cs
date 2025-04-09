using UnityEngine;

namespace Jam.Scripts.Dialogue.Runtime.Events.SO
{
    [CreateAssetMenu(menuName = "Dialogue/Events/AddReputationEvent")]
    public class AddReputationEvent : DialogueEventSO
    {
        [SerializeField] private int _amount;
        public override void RunEvent()
        {
            Debug.Log($"Add reputation {_amount}");
        }
    }
}
