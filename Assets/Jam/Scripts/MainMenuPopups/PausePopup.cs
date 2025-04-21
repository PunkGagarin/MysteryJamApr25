using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Audio.View;
using Jam.Scripts.SceneManagement;
using Jam.Scripts.Utils.Coroutine;
using Jam.Scripts.Utils.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.MainMenuPopups
{
    public class PausePopup : Popup
    {
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _playButton;

        [Inject] private CoroutineHelper _coroutineHelper;
        [Inject] private PopupManager _popupManager;
        [Inject] private SceneLoader _sceneLoader;
        [Inject] private AudioService _audioService;

        private void Awake()
        {
            _settingsButton.onClick.AddListener(OnSettingsButtonClick);
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
            _playButton.onClick.AddListener(Close);
        }

        private void OnSettingsButtonClick()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            _popupManager.OpenPopup<AudioSettingsView>();
        }

        private void OnMainMenuButtonClick()
        {
            _audioService.PlaySound(Sounds.buttonClick.ToString());
            _coroutineHelper.RunCoroutine(_sceneLoader.LoadScene(SceneEnum.MainMenu));
        }

        private void OnDestroy()
        {
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClick);
            _mainMenuButton.onClick.RemoveListener(OnMainMenuButtonClick);
            _playButton.onClick.RemoveListener(Close);
        }
    }
}
