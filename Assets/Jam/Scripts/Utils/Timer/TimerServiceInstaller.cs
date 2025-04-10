using Zenject;

namespace Jam.Scripts.Utils.Timer
{
    public class TimerServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TimerService>()
                .AsSingle()
                .NonLazy();
        }
    }
}
