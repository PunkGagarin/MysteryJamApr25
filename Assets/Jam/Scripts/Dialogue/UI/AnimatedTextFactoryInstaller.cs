using Bitwave_Labs.AnimatedTextReveal.Scripts;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Dialogue.UI
{
    public class AnimatedTextFactoryInstaller : MonoInstaller
    {
        [SerializeField] private AnimatedTextReveal _characterTextPrefab, _ghostTextPrefab;
        public override void InstallBindings()
        {
            Container
                .Bind<AnimatedTextFactory>()
                .FromNew()
                .AsSingle()
                .WithArguments(_characterTextPrefab, _ghostTextPrefab)
                .NonLazy();
        }
    }
}