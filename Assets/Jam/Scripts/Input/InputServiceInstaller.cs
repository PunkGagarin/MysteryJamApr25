using Zenject;

namespace Jam.Scripts.Input
{
    public class InputServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputService>().AsSingle();
        }
    }
}