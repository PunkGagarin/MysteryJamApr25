using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Utils.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Audio.View
{
    public class AudioSettingsView : Popup
    {
        [SerializeField] private Slider _masterVolumeSlider, _musicVolumeSlider, _soundVolumeSlider;
        [Inject] private AudioSettingsPresenter _audioSettingsPresenter;

        private void Awake()
        {
            _masterVolumeSlider.onValueChanged.AddListener(UpdateMasterVolume);
            _musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
            _soundVolumeSlider.onValueChanged.AddListener(UpdateSoundVolume);
        }

        public override void Open(bool withPause)
        {
            _audioSettingsPresenter.AttachView(this);
            base.Open(withPause);
        }
        
        private void OnDestroy()
        {
            _masterVolumeSlider.onValueChanged.RemoveListener(UpdateMasterVolume);
            _musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
            _soundVolumeSlider.onValueChanged.RemoveListener(UpdateSoundVolume);
        }
        
        private void UpdateSoundVolume(float newVolume) => 
            _audioSettingsPresenter.SetSoundVolume(newVolume);
        
        private void UpdateMusicVolume(float newVolume) =>
            _audioSettingsPresenter.SetMusicVolume(newVolume);

        private void UpdateMasterVolume(float newVolume) =>
            _audioSettingsPresenter.SetMasterVolume(newVolume);

        public void SetMasterVolume(float masterVolume) =>
            _masterVolumeSlider.value = masterVolume;
        
        public void SetSoundVolume(float soundVolume) =>
            _soundVolumeSlider.value = soundVolume;
        
        public void SetMusicVolume(float musicVolume) =>
            _musicVolumeSlider.value = musicVolume;
    }
}
