﻿using System;
using Jam.Scripts.Dialogue.Runtime.Enums;
using UnityEngine.UIElements;
#if UNITY_2022_1_OR_NEWER
#else
using UnityEditor.UIElements;
#endif

namespace Jam.Scripts.Dialogue.Runtime.SO.Values.Enums
{
    [Serializable]
    public class ContainerEndNodeType
    {
        public EnumField EnumField;
        public EndNodeType Value = EndNodeType.End;
    }
}