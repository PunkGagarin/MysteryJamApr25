using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TextAsset = UnityEngine.TextAsset;

namespace Jam.Scripts.Dialogue.Gameplay
{
    public class LanguageServiceInstaller : MonoInstaller
    {
        [SerializeField] private LanguageService languageServicePrefab;
        [SerializeField] private List<TextAsset> _localizationTextAssets;

        public override void InstallBindings()
        {
            InstallLanguageService();
            InstallLocalization();
        }

        private void InstallLanguageService()
        {
            Container.Bind<LanguageService>()
                .FromComponentInNewPrefab(languageServicePrefab)
                .AsSingle()
                .NonLazy();
        }

        private void InstallLocalization()
        {
            Container.Bind<Localization>()
                .AsSingle()
                .WithArguments(_localizationTextAssets)
                .NonLazy();
        }
    }
}