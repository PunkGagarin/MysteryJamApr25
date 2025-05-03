using UnityEngine;
using Zenject;

namespace Jam.Scripts.DayTime.Results
{
    internal class CharacterResultFactory
    {
        private DiContainer _diContainer;

        public CharacterResultFactory(DiContainer diContainer) => 
            _diContainer = diContainer;

        public CharacterResultView Create(CharacterResultView view, Transform parent) =>
            _diContainer
                .InstantiatePrefab(view, parent)
                .GetComponent<CharacterResultView>();
    }
}