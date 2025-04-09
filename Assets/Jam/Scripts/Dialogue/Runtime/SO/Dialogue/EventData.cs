using System;
using System.Collections.Generic;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Events;

namespace Jam.Scripts.Dialogue.Runtime.SO.Dialogue
{
    [Serializable]
    public class EventData : BaseData
    {
        public List<EventDataStringModifier> EventDataStringModifiers = new();
        public List<ContainerDialogueEventSO> ContainerDialogueEventSOs = new();
    }
}