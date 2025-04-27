using Zenject;

namespace Jam.Scripts.Manual
{
    public class ManualPagesFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ManualPagesFactory>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}