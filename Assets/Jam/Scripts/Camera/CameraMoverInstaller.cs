using UnityEngine;
using Zenject;

namespace Jam.Scripts.Camera
{
    public class CameraMoverInstaller : MonoInstaller
    {
        [SerializeField] private CameraMover _cameraMover;

        public override void InstallBindings()
        {
            Container
                .Bind<CameraMover>()
                .FromInstance(_cameraMover)
                .AsSingle();
        }
    }
}