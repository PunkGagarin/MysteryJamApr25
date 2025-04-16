using System;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Audio.View;
using Jam.Scripts.SceneManagement;
using Jam.Scripts.Utils.Coroutine;
using Jam.Scripts.Utils.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _startGame;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _credits;

        [Inject] private CoroutineHelper _coroutineHelper;
        [Inject] private PopupManager _popupManager;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private AudioService _audioService;

        private void Awake()
        {
            _startGame.onClick.AddListener(StartGame);
            _settings.onClick.AddListener(OpenSettings);
            _credits.onClick.AddListener(OpenCredits);
        }

        private void Start()
        {
            _audioService.PlayMusic(Sounds.mainMenuBgm.ToString());
        }

        private void StartGame()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            _coroutineHelper.RunCoroutine(_sceneLoader.LoadScene(SceneEnum.Gameplay));
        }

        private void OpenSettings()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            _popupManager.OpenPopup<AudioSettingsView>();
        }

        private void OpenCredits()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            _popupManager.OpenPopup<CreditsPopup>();
        }

        private void OnDestroy()
        {
            _startGame.onClick.RemoveListener(StartGame);
            _settings.onClick.RemoveListener(OpenSettings);
            _credits.onClick.RemoveListener(OpenCredits);
        }
    }
}