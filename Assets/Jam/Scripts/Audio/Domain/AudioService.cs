using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Audio.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Audio.Domain
{
    public class AudioService : MonoBehaviour
    {
        [Inject] private SoundRepository _soundRepository;
        [Inject] private IAudioMixerService _audioMixerService;

        private AudioSource _musicSource;
        private AudioSource _sfxInterruptibleSource;
        private AudioSource _sfxQueueSource;
        private List<AudioSource> _soundSources = new List<AudioSource>();

        private SoundElement _nextMusicClip;
        private SoundElement _nextSfxQueueClip;

        public void PlaySound(string clipName)
        {
            SoundElement clip = FindClip(clipName, SoundType.Effect);

            if (clip == null)
                return;

            AudioSource source = GetSource();

            SetSoundClip(source, clip);
        }

        public void PlaySoundInSingleAudioSource(string clipName)
        {
            SoundElement clip = FindClip(clipName, SoundType.Effect);

            if (clip == null)
                return;

            if (_sfxInterruptibleSource == null)
            {
                _sfxInterruptibleSource = gameObject.AddComponent<AudioSource>();
                _sfxInterruptibleSource.playOnAwake = false;
                _sfxInterruptibleSource.outputAudioMixerGroup = _audioMixerService.SoundMixer;
            }

            AudioSource source = _sfxInterruptibleSource;

            SetSoundClip(source, clip);
        }

        public void PlayMusic(string clipName, bool instant = false)
        {
            SoundElement clip = FindClip(clipName, SoundType.Music);

            if (clip == null)
                return;

            if (instant || _musicSource.isPlaying == false)
            {
                SetMusicClip(clip);
            }
            else
            {
                _musicSource.loop = false;
                _nextMusicClip = clip;
            }
        }

        public void PlaySfxQueue(string clipName)
        {
            SoundElement clip = FindClip(clipName, SoundType.Effect);

            if (clip == null)
                return;

            if (_sfxQueueSource == null)
            {
                _sfxQueueSource = gameObject.AddComponent<AudioSource>();
                _sfxQueueSource.playOnAwake = false;
                _sfxQueueSource.outputAudioMixerGroup = _audioMixerService.SoundMixer;
            }

            if (_sfxQueueSource.isPlaying == false)
            {
                SetSfxQueueClip(clip);
            }
            else
            {
                _sfxQueueSource.loop = false;
                _nextSfxQueueClip = clip;
            }
        }

        public void StopSfxQueue()
        {
            if (_sfxQueueSource != null)
                _sfxQueueSource.Stop();
        }

        public void SetSfxQueuePitch(float pitch)
        {
            if (_sfxQueueSource != null)
                _sfxQueueSource.pitch = pitch;   
        }

        private AudioSource GetSource()
        {
            foreach (AudioSource soundSource in _soundSources.Where(soundSource => !soundSource.isPlaying))
                return soundSource;

            return AddNewSoundSource();
        }

        private SoundElement FindClip(string clipName, SoundType soundType)
        {
            SoundElement clip;

            clip = _soundRepository.GetClip(clipName, soundType);

            return clip;
        }

        private void SetSoundClip(AudioSource soundSource, SoundElement clip)
        {
            soundSource.clip = clip.Clip;
            soundSource.volume = clip.Volume;
            soundSource.Play();
        }

        private void SetMusicClip(SoundElement clip)
        {
            _musicSource.clip = clip.Clip;
            _musicSource.volume = clip.Volume;
            _musicSource.loop = true;
            _musicSource.Play();
        }

        private void SetSfxQueueClip(SoundElement clip)
        {
            _sfxQueueSource.clip = clip.Clip;
            _sfxQueueSource.volume = clip.Volume;
            _sfxQueueSource.loop = true;
            _sfxQueueSource.Play();
        }

        private AudioSource AddNewSoundSource()
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = _audioMixerService.SoundMixer;
            _soundSources.Add(source);
            return source;
        }

        private void Awake()
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;
            _musicSource.outputAudioMixerGroup = _audioMixerService.MusicMixer;

            for (int i = 0; i < 5; i++)
                AddNewSoundSource();
        }

        private void Update()
        {
            CheckNextMusicClip();
            CheckNextSfxQueueClip();
        }

        private void CheckNextSfxQueueClip()
        {
            if (_sfxQueueSource == null ||
                _sfxQueueSource.loop ||
                _sfxQueueSource.isPlaying ||
                _nextSfxQueueClip == null)
                return;

            SetSfxQueueClip(_nextSfxQueueClip);
            _nextSfxQueueClip = null;
        }

        private void CheckNextMusicClip()
        {
            if (_musicSource.loop ||
                _musicSource.isPlaying ||
                _nextMusicClip == null)
                return;

            SetMusicClip(_nextMusicClip);
            _nextMusicClip = null;
        }
    }
}