﻿using UnityEngine;

namespace Jam.Scripts.Dialogue.Runtime.Events.SO
{
    [CreateAssetMenu(menuName = "Game Resources/Dialogue/Events/AddQuestEvent")]
    public class AddQuestEvent : DialogueEventSO
    {
        [SerializeField] private string _questName;
        public override void RunEvent()
        {
            Debug.Log($"Add quest {_questName}");
        }
    }
}
