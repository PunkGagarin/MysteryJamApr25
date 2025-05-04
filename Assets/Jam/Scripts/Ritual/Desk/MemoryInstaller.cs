using UnityEngine;
using Zenject;

namespace Jam.Scripts.Ritual.Desk
{
    public class MemoryInstaller : MonoInstaller
    {
        [SerializeField] private Memory _memory;

        public override void InstallBindings()
        {
            Container
                .Bind<Memory>()
                .FromInstance(_memory)
                .AsSingle();
        }
    }
}