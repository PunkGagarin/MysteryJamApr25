using UnityEngine;
using Zenject;

namespace Jam.Scripts.Tutorial
{
    public class TutorialInstaller : MonoInstaller
    {
        [SerializeField] private TutorialService _tutorialService;
        public override void InstallBindings()
        {
            Container
                .Bind<TutorialService>()
                .FromInstance(_tutorialService)
                .AsSingle()
                .NonLazy();
        }
    }
}