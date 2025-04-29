using Jam.Scripts.DayTime;
using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.Ritual.Inventory;
using Jam.Scripts.Shop;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.GameplayData
{
    [CreateAssetMenu(fileName = "Config Installer", menuName = "Game Resources/Config Installer")]
    public class ConfigInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private DayConfig _dayConfig;
        [SerializeField] private InventoryConfig _inventoryConfig;
        [SerializeField] private ShopConfig _shopConfig;
        [SerializeField] private LanguageConfig _languageConfig;
        
        public override void InstallBindings()
        {
            DayInstall();
            InventoryInstall();
            ShopInstall();
            LanguageInstall();
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
        private void LanguageInstall()
        {
            Container
                .Bind<LanguageConfig>()
                .FromInstance(_languageConfig)
                .AsSingle();
        }
    }
}