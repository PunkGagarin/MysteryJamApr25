using UnityEngine;
using Zenject;

namespace Jam.Scripts.Dialogue.Gameplay
{
    public class LanguageControllerInstaller : MonoInstaller
    {
        [SerializeField] private LanguageController _languageControllerPrefab;
        public override void InstallBindings()
        {
            Container.Bind<LanguageController>()
                .FromComponentInNewPrefab(_languageControllerPrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}
