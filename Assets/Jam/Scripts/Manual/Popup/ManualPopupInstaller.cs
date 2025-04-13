using Zenject;

namespace Jam.Scripts.Manual.Popup
{
    public class ManualPopupInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ManualPopupPresenterInstall();
        }
    
        private void ManualPopupPresenterInstall()
        {
            Container.BindInterfacesAndSelfTo<ManualPopupPresenter>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }

    }
}