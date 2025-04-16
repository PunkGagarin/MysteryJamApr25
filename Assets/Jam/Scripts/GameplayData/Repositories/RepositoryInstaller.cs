using Jam.Scripts.DayTime;
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
        [SerializeField] private DayConfig _dayConfig;

        public override void InstallBindings()
        {
            ManualInstall();
            QuestsInstall();
            NpcInstall();
            ConfigInstaller();
        }

        private void ConfigInstaller()
        {
            Container
                .Bind<DayConfig>()
                .FromInstance(_dayConfig)
                .AsSingle();
        }

        private void NpcInstall()
        {
            Container
                .Bind<NpcRepository>()
                .FromInstance(_npcRepository)
                .AsSingle();
        }
        
        private void QuestsInstall()
        {
            Container
                .Bind<QuestRepository>()
                .FromInstance(_questRepository)
                .AsSingle();
        }
        
        private void ManualInstall()
        {
            Container
                .Bind<ManualPagesRepository>()
                .FromInstance(_manualPagesRepository)
                .AsSingle();
        }
    }
}