using UnityEngine;
using Zenject;

namespace Jam.Scripts.Shop
{
    public class ShopSystemInstaller : MonoInstaller
    {
        [SerializeField] private ShopSystem _shopSystem;
        
        public override void InstallBindings()
        {
            Container
                .Bind<ShopSystem>()
                .FromInstance(_shopSystem)
                .AsSingle()
                .NonLazy();
        }
    }
}