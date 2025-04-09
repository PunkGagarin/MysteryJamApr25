using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.Dialogue.Runtime.SO;
using UnityEngine;

namespace Jam.Scripts
{
    public class Gameplay : MonoBehaviour
    {
        [SerializeField] private DialogueRunner _dialogueRunner;
        [SerializeField] private DialogueContainerSO _firstDialogue;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                _dialogueRunner.StartDialogue(_firstDialogue);
        }
    }
}
