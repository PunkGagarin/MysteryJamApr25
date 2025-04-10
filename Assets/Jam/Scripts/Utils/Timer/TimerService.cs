﻿using System;
using System.Collections.Generic;
using Jam.Scripts.Utils.Pause;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Utils.Timer
{
    public class TimerService : ITickable, IPauseHandler, IDisposable
    {
        private readonly List<TimerHandle> _timers = new();
        private readonly HashSet<TimerHandle> _timersToRemove = new();
        private readonly PauseService _pauseService;
        private bool _isPaused;

        [Inject]
        public TimerService(PauseService pauseService)
        {
            _pauseService = pauseService;
            pauseService.Register(this);
        }

        public TimerHandle AddTimer(float duration, Action onTimerExpire, bool isLooping = false)
        {
            var timer = new TimerHandle(duration, onTimerExpire, isLooping);
            _timers.Add(timer);
            return timer;
        }

        public void RemoveTimer(TimerHandle timerToRemove)
        {
            if (_timers.Contains(timerToRemove))
                _timersToRemove.Add(timerToRemove);
        }

        public void ClearAllTimers()
        {
            _timers.Clear();
            _timersToRemove.Clear();
        }

        public void Tick()
        {
            if (_isPaused) 
                return;

            float deltaTime = Time.deltaTime;
            for (int i = _timers.Count - 1; i >= 0; i--)
            {
                var timer = _timers[i];
               
                if (_timersToRemove.Contains(timer)) 
                    continue;

                timer.Tick(deltaTime);

                if (timer.RemainingTime <= 0)
                {
                    timer.OnTimerExpire?.Invoke();

                    if (timer.IsLooping)
                    {
                        timer.Reset();
                    }
                    else
                    {
                        _timersToRemove.Add(timer);
                    }
                }
            }
            
            if (_timersToRemove.Count > 0)
            {
                foreach (TimerHandle timer in _timersToRemove)
                {
                    _timers.Remove(timer);
                }

                _timersToRemove.Clear();
            }
        }

        public void SetPaused(bool isPaused)
        {
            _isPaused = isPaused;
        }

        public void Dispose()
        {
            ClearAllTimers();
            _pauseService.Unregister(this);
        }
    }
}
