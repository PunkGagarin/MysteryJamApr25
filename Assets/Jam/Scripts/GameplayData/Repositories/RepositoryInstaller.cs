using Jam.Scripts.Manual.Popup;
using Jam.Scripts.Npc.Data;
using Jam.Scripts.Quests.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.GameplayData.Repositories
{
    [CreateAssetMenu(fileName = "Repository Installer", menuName = "Game Resources/Repository Installer")]
    public class RepositoryInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private ManualPagesRepository _manualPagesRepository;
        [SerializeField] private QuestRepository _questRepository;
        [SerializeField] private NpcRepository _npcRepository;

        public override void InstallBindings()
        {
            Container
                .Bind<ManualPagesRepository>()
                .FromInstance(_manualPagesRepository)
                .AsSingle();
            
            Container
                .Bind<QuestRepository>()
                .FromInstance(_questRepository)
                .AsSingle();
            
            Container
                .Bind<NpcRepository>()
                .FromInstance(_npcRepository)
                .AsSingle();
        }
    }
}