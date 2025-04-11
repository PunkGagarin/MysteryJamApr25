using System;
using System.Collections.Generic;
using Bitwave_Labs.AnimatedTextReveal.Scripts;
using DG.Tweening;
using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.Dialogue.Runtime.Enums;
using Jam.Scripts.Utils.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Jam.Scripts.Dialogue.UI
{
    public class DialogueView : Popup
    {
        [SerializeField] private RectTransform _dialoguePanel;
        [Header("Text")]
        [SerializeField] private TMP_Text _nameText;
        //[SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private AnimatedTextReveal _dialogueText;
        [Header("Button")]
        [SerializeField] private GameObject _buttonContentPanel;
        [SerializeField] private ButtonController _buttonPrefab;
        [Header("Button color")]
        [SerializeField] private Color _textDisableColor;
        [SerializeField] private Color _buttonDisableColor;
        [Header("Interactable")]
        [SerializeField] private Color _textInteractableColor;

        private List<ButtonController> _buttons = new();

        public override void Open(bool withPause)
        {
            _dialoguePanel.gameObject.SetActive(true);
            _dialoguePanel.DOScale(1f, 1f).SetEase(Ease.Linear);
            base.Open(withPause);
        }

        public override void Close()
        {
            _dialoguePanel.DOScale(0f, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _dialoguePanel.gameObject.SetActive(true);
                base.Close();
            });
        }

        public void SetName(string nameText) =>
            _nameText.text = nameText;

        public void SetText(string text, Action onComplete = null)
        {
            _dialogueText.ResetTextVisibility();
            _dialogueText.TextMesh.text = text;
            StartCoroutine(_dialogueText.FadeInText(onComplete));
        }

        public void SetButtons(List<DialogueButtonContainer> buttonContainers)
        {
            HideButtons();
            CheckAndFillDialogueAnswerRooms(buttonContainers.Count);
            for (int i = 0; i < buttonContainers.Count; i++)
            {
                Button currentButton = _buttons[i].GetComponent<Button>();
                currentButton.onClick = new Button.ButtonClickedEvent();
                currentButton.interactable = true;
                _buttons[i].ButtonText.color = _textInteractableColor;
                if (buttonContainers[i].ConditionCheck || buttonContainers[i].ChoiceStateType == ChoiceStateType.GrayOut)
                {
                    _buttons[i].SetText($"{i + 1}: {buttonContainers[i].Text}");
                    _buttons[i].gameObject.SetActive(true);

                    if (!buttonContainers[i].ConditionCheck)
                    {
                        currentButton.interactable = false;
                        _buttons[i].ButtonText.color = _textDisableColor;
                        var colors = currentButton.colors;
                        colors.disabledColor = _buttonDisableColor;
                        currentButton.colors = colors;
                    }
                    else
                    {
                        currentButton.onClick.AddListener(buttonContainers[i].UnityAction);
                    }
                }
            }
        }

        public void HideButtons() => 
            _buttons.ForEach(button => button.gameObject.SetActive(false));

        private void Update()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                if (Keyboard.current[Key.Digit1 + i].wasPressedThisFrame)
                {
                    Button currentButton = _buttons[i].GetComponent<Button>();
                    if (currentButton.gameObject.activeSelf) currentButton.onClick.Invoke();
                }
            }
        }

        private void CheckAndFillDialogueAnswerRooms(int buttonsCountToCreate)
        {
            if (_buttons.Count >= buttonsCountToCreate) return;
            int buttonsToCreate = buttonsCountToCreate - _buttons.Count;
            for (int i = 0; i < buttonsToCreate; i++)
            {
                ButtonController button = Instantiate(_buttonPrefab, _buttonContentPanel.transform);
                _buttons.Add(button);
                button.gameObject.SetActive(false);
            }
        }
    }
}