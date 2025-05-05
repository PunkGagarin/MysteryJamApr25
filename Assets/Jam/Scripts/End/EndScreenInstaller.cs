using UnityEngine;
using Zenject;

namespace Jam.Scripts.End
{
    public class EndScreenInstaller : MonoInstaller
    {
        [SerializeField] private EndScreen _endScreen;

        public override void InstallBindings()
        {
            Container
                .Bind<EndScreen>()
                .FromInstance(_endScreen)
                .AsSingle();
        }
    }
}