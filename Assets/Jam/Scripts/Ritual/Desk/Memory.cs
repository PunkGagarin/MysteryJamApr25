using System;
using System.Collections.Generic;
using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Ritual.Desk
{
    public class Memory : MonoBehaviour
    {
        [SerializeField] private List<Disk> _disks;
        [Inject] private AudioService _audioService;
        
        private MemoryConfig _memoryConfig;
        private List<int> _sequence = new List<int>();
        private int _inputIndex = 0;
        private bool _canInput = false;

        private Action _onWin, _onLose;

        public void SetMemoryConfig(MemoryConfig memoryConfig) => 
            _memoryConfig = memoryConfig;

        public void StartMemoryGame(Action onWin, Action onLose)
        {
            _onWin = onWin;
            _onLose = onLose;
            
            _canInput = false;
            _sequence.Clear();

            int lastIndex = -1;

            for (int i = 0; i < _memoryConfig.ClicksAmount; i++)
            {
                int index;

                do
                {
                    index = Random.Range(0, _disks.Count);
                }
                while (index == lastIndex);

                lastIndex = index;
                _sequence.Add(index);
            }

            bool allSame = true;
            for (int i = 1; i < _sequence.Count; i++)
            {
                if (_sequence[i] != _sequence[0])
                {
                    allSame = false;
                    break;
                }
            }
            if (allSame)
            {
                StartMemoryGame(_onWin, _onLose);
                return;
            }

            HighlightButton(0);
        }

        private void AllowInput()
        {
            _canInput = true;
            _inputIndex = 0;
        }

        private void OnButtonPress(Disk disk)
        {
            if (!_canInput) 
                return;

            switch (disk.Type)
            {
                case ReagentType.Age:
                    _audioService.PlaySound(Sounds.ageRitualItemHighlight);
                    break;
                case ReagentType.Sex:
                    _audioService.PlaySound(Sounds.sexRitualItemHighlight);
                    break;
                default:
                    _audioService.PlaySound(Sounds.raceRitualItemHighlight);
                    break;
            }
            
            disk.ReagentVisual.transform.DOShakePosition(.2f, .5f);
            
            if (_sequence[_inputIndex] == _disks.IndexOf(disk))
            {
                _inputIndex++;
                if (_inputIndex >= _sequence.Count)
                {
                    EndGameEvent(_onWin);
                }
            }
            else
            {
                EndGameEvent(_onLose);
            }
        }

        private void EndGameEvent(Action endEvent)
        {
            _canInput = false;
            endEvent?.Invoke();
        }

        private void HighlightButton(int sequenceIndex)
        {
            if (sequenceIndex >= _sequence.Count)
            {
                AllowInput();
                return;
            }
            
            int diskIndex = _sequence[sequenceIndex];

            _disks[diskIndex].HighLight(() => HighlightButton(sequenceIndex + 1), _memoryConfig.HighLightTime);
        }

        private void Awake()
        {
            foreach (var disk in _disks) 
                disk.DiskClicked += OnButtonPress;
        }

        private void OnDestroy()
        {
            foreach (var disk in _disks) 
                disk.DiskClicked -= OnButtonPress;
        }
    }
}