﻿using Jam.Scripts.DayTime;
using Jam.Scripts.Ritual.Inventory;
using Jam.Scripts.Shop;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.GameplayData
{
    [CreateAssetMenu(fileName = "Gameplay Config Installer", menuName = "Game Resources/Gameplay Config Installer")]
    public class GameplayConfigInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private DayConfig _dayConfig;
        [SerializeField] private InventoryConfig _inventoryConfig;
        [SerializeField] private ShopConfig _shopConfig;

        public override void InstallBindings()
        {
            DayInstall();
            InventoryInstall();
            ShopInstall();
        }

        private void ShopInstall()
        {
            Container
                .Bind<ShopConfig>()
                .FromInstance(_shopConfig)
                .AsSingle();
        }

        private void InventoryInstall()
        {
            Container
                .Bind<InventoryConfig>()
                .FromInstance(_inventoryConfig)
                .AsSingle();
        }

        private void DayInstall()
        {
            Container
                .Bind<DayConfig>()
                .FromInstance(_dayConfig)
                .AsSingle();
        }
    }
}