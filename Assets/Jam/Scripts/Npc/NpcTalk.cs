using System;
using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.Dialogue.Runtime.SO;
using Jam.Scripts.Dialogue.UI;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Npc
{
    public class NpcTalk : MonoBehaviour
    {
        [Inject] private DialogueRunner _dialogueRunner;
        
        public bool DialogueInProcess { get; private set; }

        public event Action OnDialogueComplete;

        public void Talk(DialogueContainerSO dialogueContainer)
        {
            DialogueInProcess = true;
            _dialogueRunner.StartDialogue(dialogueContainer, DialogueComplete);
        }
        
        private void DialogueComplete()
        {
            OnDialogueComplete?.Invoke();
            DialogueInProcess = false;
        }
    }
}