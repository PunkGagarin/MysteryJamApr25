using System;
using Jam.Scripts.Audio.Data;
using Jam.Scripts.Audio.View;
using Zenject;

namespace Jam.Scripts.Audio.Domain
{
    public class AudioSettingsPresenter : IInitializable, IDisposable
    {
        private AudioSettingsModel _audioSettingsModel;
        
        private IAudioMixerService _audioMixerService;
        
        private AudioSettingsView _audioSettingsView;

        public AudioSettingsPresenter(AudioSettingsModel audioSettingsModel, IAudioMixerService audioMixerService)
        {
            _audioSettingsModel = audioSettingsModel;
            _audioMixerService = audioMixerService;

            _audioSettingsModel.MasterVolumeChanged += UpdateMasterVolume;
            _audioSettingsModel.SoundVolumeChanged += UpdateSoundVolume;
            _audioSettingsModel.MusicVolumeChanged += UpdateMusicVolume;
        }
        
        public void Initialize()
        {
            _audioMixerService.SetMasterVolume(_audioSettingsModel.MusicVolume, _audioSettingsModel.SoundVolume, _audioSettingsModel.MasterVolume);
        }
        
        public void AttachView(AudioSettingsView view)
        {
            if (_audioSettingsView != null)
                return;
            
            _audioSettingsView = view;
            SyncWithView();
        }
        
        public void SetMasterVolume(float volume) => 
            _audioSettingsModel.SetMasterVolume(volume);
        
        public void SetSoundVolume(float volume) => 
            _audioSettingsModel.SetSoundVolume(volume);
        
        public void SetMusicVolume(float volume) =>
            _audioSettingsModel.SetMusicVolume(volume);
        
        private void UpdateMasterVolume(float newVolume) =>
            _audioMixerService.SetMasterVolume(_audioSettingsModel.MusicVolume, _audioSettingsModel.SoundVolume, newVolume);
        private void UpdateSoundVolume(float newVolume) => 
            _audioMixerService.SetSoundVolume(newVolume, _audioSettingsModel.MasterVolume);
        private void UpdateMusicVolume(float newVolume) => 
            _audioMixerService.SetMusicVolume(newVolume, _audioSettingsModel.MasterVolume);
        
        private void SyncWithView()
        {
            _audioSettingsView.SetMasterVolume(_audioSettingsModel.MasterVolume);
            _audioSettingsView.SetSoundVolume(_audioSettingsModel.SoundVolume);
            _audioSettingsView.SetMusicVolume(_audioSettingsModel.MusicVolume);
        }

        public void Dispose()
        {
            _audioSettingsModel.MasterVolumeChanged -= UpdateMasterVolume;
            _audioSettingsModel.SoundVolumeChanged -= UpdateSoundVolume;
            _audioSettingsModel.MusicVolumeChanged -= UpdateMusicVolume;
        }
    }
}
