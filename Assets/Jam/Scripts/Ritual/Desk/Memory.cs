using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Ritual.Desk
{
    public class Memory : MonoBehaviour
    {
        [SerializeField] private List<Disk> _disks;
        
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

            for (int i = 0; i < _memoryConfig.ClicksAmount; i++)
            {
                int index = Random.Range(0, _disks.Count);
                _sequence.Add(index);
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