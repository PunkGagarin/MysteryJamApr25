using System;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Enums;

namespace Jam.Scripts.Dialogue.Runtime.SO.Values.Events
{
    [Serializable]
    public class EventDataStringModifier
    {
        public ContainerEventString StringEventText = new();
        public ContainerFloat Number = new();
        public ContainerStringEventModifierType StringEventModifierType = new();
    }
}