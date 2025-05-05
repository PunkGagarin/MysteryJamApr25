using Jam.Scripts.VFX;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.UI
{
    public class GameplayUIInstaller : MonoInstaller
    {
        [SerializeField] private GameplayOverlayUI _gameplayOverlay;
        [SerializeField] private PointerFirefly _pointerFirefly;
        [SerializeField] private ImageFader _dayFader;

        public override void InstallBindings()
        {
            Container
                .Bind<GameplayOverlayUI>()
                .FromInstance(_gameplayOverlay)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<ImageFader>()
                .FromInstance(_dayFader)
                .AsSingle()
                .NonLazy();

            Container
                .Bind<PointerFirefly>()
                .FromInstance(_pointerFirefly)
                .AsSingle()
                .NonLazy();
        }
    }
}