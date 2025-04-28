using System.Collections.Generic;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.Dialogue.Runtime.SO;
using UnityEngine.Events;

namespace Jam.Scripts.Dialogue.Gameplay
{
    public class DialogueButtonContainer
    {
        public UnityAction UnityAction { get; set; }
        public List<LanguageGeneric<string>> Text { get; set; }
        public bool ConditionCheck { get; set; }
        public ChoiceStateType ChoiceStateType { get; set; }
    }
}