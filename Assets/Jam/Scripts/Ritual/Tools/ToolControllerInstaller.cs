using UnityEngine;
using Zenject;

namespace Jam.Scripts.Ritual.Tools
{
    public class ToolControllerInstaller : MonoInstaller
    {
        [SerializeField] private ToolController _toolController;

        public override void InstallBindings()
        {
            Container
                .Bind<ToolController>()
                .FromInstance(_toolController)
                .AsSingle();
        }
    }
}