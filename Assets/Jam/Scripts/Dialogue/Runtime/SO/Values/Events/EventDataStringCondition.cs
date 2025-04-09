using System;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Enums;

namespace Jam.Scripts.Dialogue.Runtime.SO.Values.Events
{
    [Serializable]
    public class EventDataStringCondition
    { 
        public ContainerEventString StringEventText = new();
        public ContainerFloat Number = new();
        public ContainerStringEventConditionType StringEventConditionType = new();
    }
}