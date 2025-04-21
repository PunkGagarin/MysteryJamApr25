using UnityEngine;
using Zenject;

namespace Jam.Scripts.Dialogue.UI
{
    public class AnimatedTextFactory
    {
        private AnimatedTextReveal _characterTextPrefab, _ghostTextPrefab;
        private DiContainer _diContainer;

        public AnimatedTextFactory(DiContainer diContainer, AnimatedTextReveal characterTextPrefab, AnimatedTextReveal ghostTextPrefab)
        {
            _characterTextPrefab = characterTextPrefab;
            _ghostTextPrefab = ghostTextPrefab;
            _diContainer = diContainer;
        }

        public AnimatedTextReveal Create(bool isGhost, Transform parentContainer) => isGhost
            ? _diContainer.InstantiatePrefab(_ghostTextPrefab, parentContainer).GetComponent<AnimatedTextReveal>()
            : _diContainer.InstantiatePrefab(_characterTextPrefab, parentContainer).GetComponent<AnimatedTextReveal>();
    }
}