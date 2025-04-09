using UnityEngine;
using Zenject;

namespace Jam.Scripts.Dialogue.Runtime.Events
{
    public class GameEventsInstaller : MonoInstaller
    {
        [SerializeField] private GameEvents _gameEventsPrefab;

        public override void InstallBindings()
        {
            Container.Bind<GameEvents>()
                .FromComponentInNewPrefab(_gameEventsPrefab)
                .AsSingle()
                .NonLazy();
        }
    }
}
