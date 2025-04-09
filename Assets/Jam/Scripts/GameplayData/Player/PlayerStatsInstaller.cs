using Zenject;

namespace Jam.Scripts.GameplayData.Player
{
    public class PlayerStatsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerStats>().AsSingle().NonLazy();
        }
    }
}
