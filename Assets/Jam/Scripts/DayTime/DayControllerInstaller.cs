using UnityEngine;
using Zenject;

namespace Jam.Scripts.DayTime
{
    public class DayControllerInstaller : MonoInstaller
    {
        [SerializeField] private DayController _dayController;
        
        public override void InstallBindings()
        {
            Container
                .Bind<DayController>()
                .FromInstance(_dayController)
                .AsSingle()
                .NonLazy();
        }
    }
}