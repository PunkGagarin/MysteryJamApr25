using Zenject;

namespace Jam.Scripts.GameplayData.Player
{
    public class PlayerStatsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerStatsPresenter>().AsSingle().NonLazy();
        }
    }
}
