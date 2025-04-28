using UnityEngine;
using Zenject;

namespace Jam.Scripts.Dialogue.UI
{
    public class TextFactory
    {
        private AnimatedTextReveal _characterTextPrefab, _ghostTextPrefab;
        private AnswerButtonHistory _historyText;
        private ButtonController _buttonController;
        private DiContainer _diContainer;

        public TextFactory(DiContainer diContainer, AnimatedTextReveal characterTextPrefab, AnimatedTextReveal ghostTextPrefab, AnswerButtonHistory answerButtonHistory, ButtonController buttonController)
        {
            _diContainer = diContainer;
            _characterTextPrefab = characterTextPrefab;
            _ghostTextPrefab = ghostTextPrefab;
            _historyText = answerButtonHistory;
            _buttonController = buttonController;
        }

        public AnimatedTextReveal CreateAnimatedText(bool isGhost, Transform parentContainer) => isGhost
            ? _diContainer.InstantiatePrefab(_ghostTextPrefab, parentContainer).GetComponent<AnimatedTextReveal>()
            : _diContainer.InstantiatePrefab(_characterTextPrefab, parentContainer).GetComponent<AnimatedTextReveal>();

        public AnswerButtonHistory CreateHistoryButton(Transform parentContainer) =>
            _diContainer.InstantiatePrefab(_historyText, parentContainer).GetComponent<AnswerButtonHistory>();
        
        public ButtonController CreateButton(Transform parentContainer) =>
            _diContainer.InstantiatePrefab(_buttonController, parentContainer).GetComponent<ButtonController>();
    }
}