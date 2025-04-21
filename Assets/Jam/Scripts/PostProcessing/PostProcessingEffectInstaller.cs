using UnityEngine;
using Zenject;

namespace Jam.Scripts.PostProcessing
{
    public class PostProcessingEffectInstaller : MonoInstaller
    {
        [SerializeField] private GhostResponseEffect _ghostResponseEffect;

        public override void InstallBindings()
        {
            GhostResponseEffectInstall();
        }

        private void GhostResponseEffectInstall()
        {
            Container.Bind<GhostResponseEffect>()
                .FromInstance(_ghostResponseEffect)
                .AsSingle();
        }
    }
}