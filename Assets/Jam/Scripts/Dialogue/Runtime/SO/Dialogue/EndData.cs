using System;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Enums;

namespace Jam.Scripts.Dialogue.Runtime.SO.Dialogue
{
    [Serializable]
    public class EndData : BaseData
    {
        public ContainerEndNodeType EndNodeType = new();
    }
}