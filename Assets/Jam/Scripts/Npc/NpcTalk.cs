using System;
using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.Dialogue.Runtime.SO;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Npc
{
    public class NpcTalk : MonoBehaviour
    {
        [Inject] private DialogueRunner _dialogueRunner;
        
        public void Talk(DialogueContainerSO dialogueContainer, Action leaveAction) =>
            _dialogueRunner.StartDialogue(dialogueContainer, leaveAction);
    }
}