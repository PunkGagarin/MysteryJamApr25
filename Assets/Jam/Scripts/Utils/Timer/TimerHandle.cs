﻿using System;
using UnityEngine;

namespace Jam.Scripts.Utils.Timer
{
    public class TimerHandle
    {
        private float _speedMultiplier = 1f;
        private bool _isLooping;

        public float RemainingTime { get; private set; }
        public float Duration { get; }
        public bool IsLooping => _isLooping;
        public Action OnTimerExpire { get; private set; }

        public float Progress => Mathf.Clamp01(RemainingTime / Duration);    
        
        public float SpeedMultiplier
        {
            get => _speedMultiplier;
            set => _speedMultiplier = Mathf.Max(0, value);
        }
        
        public bool IsActive => RemainingTime > 0;
        
        public TimerHandle(float duration, Action onTimerExpire, bool isLooping)
        {
            Duration = duration;
            RemainingTime = duration;
            OnTimerExpire = onTimerExpire;
            _isLooping = isLooping;
        }
        
        public void EarlyComplete(bool withoutAction = false)
        {
            if (withoutAction)
                OnTimerExpire = null;
            
            RemainingTime = 0;
        }

        public void AddTime(float additionalTime) => 
            RemainingTime = Mathf.Min(RemainingTime + additionalTime, Duration);

        public void RemoveTime(float additionalTime) => 
            RemainingTime = Mathf.Max(RemainingTime - additionalTime, 0);

        public void Tick(float deltaTime) => 
            RemainingTime -= deltaTime * SpeedMultiplier;

        public void Reset()
        {
            RemainingTime = Duration;
        }

        public void FinalizeTimer() => 
            _isLooping = false;
    }
}
