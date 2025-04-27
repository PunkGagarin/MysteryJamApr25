using UnityEngine;
using Zenject;

namespace Jam.Scripts.Manual
{
    public class ManualPagesFactory
    {
        private DiContainer _diContainer;

        public ManualPagesFactory(DiContainer diContainer) => 
            _diContainer = diContainer;

        public Page Create(ManualPage manualPage, Transform parent) =>
            _diContainer
                .InstantiatePrefab(manualPage.Page, parent)
                .GetComponent<Page>();
    }
}