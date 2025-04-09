using System;
using System.Collections.Generic;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Events;

namespace Jam.Scripts.Dialogue.Runtime.SO.Dialogue
{
    [Serializable]
    public class BranchData : BaseData
    {
        public string TrueGuidNode;
        public string FalseGuidNode;
        public List<EventDataStringCondition> EventDataStringConditions = new();
    }
}