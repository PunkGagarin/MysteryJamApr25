using Zenject;

namespace Jam.Scripts.Quests
{
    public class QuestsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<QuestPresenter>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}
