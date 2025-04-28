using UnityEngine;
using Zenject;

namespace Jam.Scripts.Dialogue.UI
{
    public class TextFactoryInstaller : MonoInstaller
    {
        [SerializeField] private AnimatedTextReveal _characterTextPrefab, _ghostTextPrefab;
        [SerializeField] private AnswerButtonHistory _answerButtonHistoryPrefab;
        [SerializeField] private ButtonController _buttonControllerPrefab;
        public override void InstallBindings()
        {
            Container
                .Bind<TextFactory>()
                .FromNew()
                .AsSingle()
                .WithArguments(_characterTextPrefab, _ghostTextPrefab, _answerButtonHistoryPrefab, _buttonControllerPrefab)
                .NonLazy();
        }
    }
}