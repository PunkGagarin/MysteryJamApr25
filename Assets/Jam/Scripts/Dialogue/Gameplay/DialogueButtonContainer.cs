using Jam.Scripts.Dialogue.Runtime.Enums;
using UnityEngine.Events;

namespace Jam.Scripts.Dialogue.Gameplay
{
    public class DialogueButtonContainer
    {
        public UnityAction UnityAction { get; set; }
        public string Text { get; set; }
        public bool ConditionCheck { get; set; }
        public ChoiceStateType ChoiceStateType { get; set; }
    }
}