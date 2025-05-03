using Zenject;

namespace Jam.Scripts.DayTime.Results
{
    public class CharacterResultFactoryInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<CharacterResultFactory>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}