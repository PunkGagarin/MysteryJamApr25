using Jam.Scripts.DayTime;
using Jam.Scripts.Manual;
using Jam.Scripts.Npc.Data;
using Jam.Scripts.Quests.Data;
using Jam.Scripts.Ritual.Inventory;
using Jam.Scripts.Ritual.Inventory.Reagents;
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
        [SerializeField] private ReagentRepository _reagentRepository;
        [SerializeField] private InventoryConfig _inventoryConfig;

        public override void InstallBindings()
        {
            ManualInstall();
            QuestsInstall();
            NpcInstall();
            ConfigInstall();
            ComponentInstall();
            InventoryInstall();
        }

        private void InventoryInstall()
        {
            Container
                .Bind<InventoryConfig>()
                .FromInstance(_inventoryConfig)
                .AsSingle();
        }

        private void ComponentInstall()
        {
            Container
                .Bind<ReagentRepository>()
                .FromInstance(_reagentRepository)
                .AsSingle();
        }

        private void ConfigInstall()
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