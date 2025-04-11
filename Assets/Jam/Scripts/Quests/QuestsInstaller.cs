using Jam.Scripts.Quests.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Quests
{
    public class QuestsInstaller : MonoInstaller
    {
        [SerializeField] private QuestRepository _questRepository;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<QuestPresenter>()
                .FromNew()
                .AsSingle()
                .WithArguments(_questRepository)
                .NonLazy();
        }
    }
}
