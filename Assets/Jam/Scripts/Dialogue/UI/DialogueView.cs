using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.Dialogue.Runtime.SO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Dialogue.UI
{
    public class DialogueView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _contentContainer;
        [SerializeField] private RectTransform _dialoguePanel;
        [SerializeField] private ScrollRect _scrollRect;
        [Header("Button")]
        [SerializeField] private Transform _buttonContentPanel;
        [SerializeField] private Button _continueButton;
        [Header("Button color")]
        [SerializeField] private Color _textDisableColor;
        [SerializeField] private Color _buttonDisableColor;
        [Header("Interactable")]
        [SerializeField] private Color _textInteractableColor;

        [Inject] private TextFactory _textFactory;
        [Inject] private AudioService _audioService;
        [Inject] private LanguageService _languageService;

        private List<ButtonController> _buttons = new();
        private AnimatedTextReveal _currentText;
        private List<GameObject> _dialogueObjects = new();

        public void Open() => 
            _dialoguePanel.gameObject.SetActive(true);

        public void Close()
        {
            _dialoguePanel.gameObject.SetActive(false);
            foreach (var dialogueObject in _dialogueObjects) 
                Destroy(dialogueObject);
        }
        public void SetText(List<LanguageGeneric<string>> text, bool isGhost, Action onComplete = null)
        {
            var currentText = _textFactory.CreateAnimatedText(isGhost, _contentContainer);
            _dialogueObjects.Add(currentText.gameObject);
            _currentText = currentText;
            ScrollContent();
            _currentText.ResetTextVisibility();
            _currentText.SetText(text);
            _currentText.ShowText(onComplete);
        }

        public void SetButtons(List<DialogueButtonContainer> buttonContainers)
        {
            HideButtons();

            if (buttonContainers.Count == 1 && buttonContainers[0].Text.Any(languageGeneric => languageGeneric.LanguageGenericType is "Continue" or "Продолжить"))
            {
                _continueButton.onClick.AddListener(() =>
                {
                    _audioService.PlaySound(Sounds.buttonClick.ToString());
                    buttonContainers[0].UnityAction?.Invoke();
                    _continueButton.onClick.RemoveAllListeners();
                    _continueButton.onClick.AddListener(FastFinishWriter);
                });
                return;
            }

            CheckAndFillDialogueAnswerRooms(buttonContainers.Count);
            for (int i = 0; i < buttonContainers.Count; i++)
            {
                Button currentButton = _buttons[i].Button;
                currentButton.onClick = new Button.ButtonClickedEvent();
                currentButton.interactable = true;
                _buttons[i].ButtonText.color = _textInteractableColor;
                var buttonContainer = buttonContainers[i];
                if (buttonContainer.ConditionCheck || buttonContainer.ChoiceStateType == ChoiceStateType.GrayOut)
                {
                    _buttons[i].gameObject.SetActive(true);
                    _buttons[i].SetText(i + 1, buttonContainer.Text);

                    if (!buttonContainer.ConditionCheck)
                    {
                        currentButton.interactable = false;
                        _buttons[i].ButtonText.color = _textDisableColor;
                        var colors = currentButton.colors;
                        colors.disabledColor = _buttonDisableColor;
                        currentButton.colors = colors;
                    }
                    else
                    {
                        currentButton.onClick.AddListener(() =>
                        {
                            _audioService.PlaySound(Sounds.buttonClick.ToString());
                            AddButtonText(buttonContainer.Text);
                            buttonContainer.UnityAction?.Invoke();
                            HideButtons();
                        });
                    }
                }
            }
        }

        private void AddButtonText(List<LanguageGeneric<string>> text)
        {
            var answerButtonHistory = _textFactory.CreateHistoryButton(_contentContainer);
            _dialogueObjects.Add(answerButtonHistory.gameObject);
            answerButtonHistory.SetText(text);
            ScrollContent();
        }

        private void ScrollContent()
        {
            Canvas.ForceUpdateCanvases();

            _scrollRect.DONormalizedPos(Vector2.zero, .3f);
        }

        public void HideButtons() =>
            _buttons.ForEach(button => button.gameObject.SetActive(false));

        private void Update()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                if (Keyboard.current[Key.Digit1 + i].wasPressedThisFrame)
                {
                    Button currentButton = _buttons[i].Button;
                    if (currentButton.gameObject.activeSelf)
                        currentButton.onClick.Invoke();
                }
            }
        }

        private void CheckAndFillDialogueAnswerRooms(int buttonsCountToCreate)
        {
            if (_buttons.Count >= buttonsCountToCreate) return;
            int buttonsToCreate = buttonsCountToCreate - _buttons.Count;
            for (int i = 0; i < buttonsToCreate; i++)
            {
                ButtonController button = _textFactory.CreateButton(_buttonContentPanel);
                _buttons.Add(button);
                button.gameObject.SetActive(false);
            }
        }

        private void FastFinishWriter() =>
            _currentText?.FinishCoroutine();

        private void Awake()
        {
            _canvas.worldCamera = UnityEngine.Camera.main;
            _continueButton.onClick.AddListener(FastFinishWriter);
        }

        private void OnDestroy()
        {
            _continueButton.onClick.RemoveAllListeners();
        }
    }
}