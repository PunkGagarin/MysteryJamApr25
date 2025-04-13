using Jam.Scripts.Ritual.Components;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Ritual
{
    public class RitualInstaller : MonoInstaller
    {
        [SerializeField] private RitualController _ritualController;
        [SerializeField] private ComponentsAnimationController _componentsAnimationControllerPrefab;
        public override void InstallBindings()
        {
            RitualControllerInstall();
            ComponentsAnimationControllerInstall();
        }
        
        private void ComponentsAnimationControllerInstall()
        {
            Container.Bind<ComponentsAnimationController>()
                .FromInstance(_componentsAnimationControllerPrefab)
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

    }
}
