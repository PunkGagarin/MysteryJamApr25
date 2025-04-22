using UnityEngine;
using Zenject;

namespace Jam.Scripts.Npc
{
    public class CharacterInstaller : MonoInstaller
    {
        [SerializeField] private Character _character;
        
        public override void InstallBindings()
        {
            Container
                .Bind<Character>()
                .FromInstance(_character)
                .AsSingle()
                .NonLazy();
        }
    }
}