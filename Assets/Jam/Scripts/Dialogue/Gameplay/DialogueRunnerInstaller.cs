using UnityEngine;
using Zenject;

namespace Jam.Scripts.Dialogue.Gameplay
{
    public class DialogueRunnerInstaller : MonoInstaller
    {
        [SerializeField] private DialogueRunner _dialogueRunner;
        
        public override void InstallBindings()
        {
            Container.Bind<DialogueRunner>()
                .FromInstance(_dialogueRunner)
                .AsSingle();
        }
    }
}
