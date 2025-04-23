using UnityEngine;
using Zenject;

namespace Jam.Scripts.Manual
{
    public class ManualInstaller : MonoInstaller
    {
        [SerializeField] private ManualBookItem _manualBookItem;
        
        public override void InstallBindings()
        {
            Container
                .Bind<ManualBookItem>()
                .FromInstance(_manualBookItem)
                .AsSingle()
                .NonLazy();
        }
    }
}