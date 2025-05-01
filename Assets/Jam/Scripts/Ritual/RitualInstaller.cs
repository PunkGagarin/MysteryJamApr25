using Jam.Scripts.Ritual.Inventory;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Ritual
{
    public class RitualInstaller : MonoInstaller
    {
        [SerializeField] private RitualController _ritualController;
        [SerializeField] private ReagentAnimationController _reagentAnimationControllerPrefab;
        [SerializeField] private InventorySystem _inventorySystem;
        [SerializeField] private ReagentDragger _reagentDragger;
        public override void InstallBindings()
        {
            ComponentsAnimationControllerInstall();
            DraggerInstall();
            RitualControllerInstall();
            InventoryInstall();
        }

        private void DraggerInstall()
        {
            Container.Bind<ReagentDragger>()
                .FromInstance(_reagentDragger)
                .AsSingle()
                .NonLazy();
        }

        private void ComponentsAnimationControllerInstall()
        {
            Container.Bind<ReagentAnimationController>()
                .FromInstance(_reagentAnimationControllerPrefab)
                .AsSingle()
                .NonLazy();
        }
        
        private void RitualControllerInstall()
        {
            Container.Bind<RitualController>()
                .FromInstance(_ritualController)
                .AsSingle()
                .NonLazy();
        }

        private void InventoryInstall()
        {
            Container
                .Bind<InventorySystem>()
                .FromInstance(_inventorySystem)
                .AsSingle()
                .NonLazy();
        }
    }
}
