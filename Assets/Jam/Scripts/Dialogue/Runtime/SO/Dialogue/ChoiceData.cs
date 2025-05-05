using System;
using System.Collections.Generic;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Enums;
using Jam.Scripts.Dialogue.Runtime.SO.Values.Events;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using UnityEngine;
using UnityEngine.UIElements;

namespace Jam.Scripts.Dialogue.Runtime.SO.Dialogue
{
    [Serializable]
    public class ChoiceData : BaseData
    {
        public TextField TextField { get; set; }
#if UNITY_EDITOR
        public ObjectField ObjectField { get; set; }
#endif
        public ContainerChoiceStateType ChoiceStateType = new();
        public List<LanguageGeneric<string>> Text = new();
        public List<LanguageGeneric<AudioClip>> AudioClips = new();
        public List<EventDataStringCondition> EventDataStringConditions = new();
    }
}