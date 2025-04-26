using UnityEngine;

namespace Jam.Scripts.Dialogue.Runtime.Events.SO
{
    public class AddQuestEvent : DialogueEventSO
    {
        [SerializeField] private string _questName;
        public override void RunEvent()
        {
            Debug.Log($"Add quest {_questName}");
        }
    }
}
