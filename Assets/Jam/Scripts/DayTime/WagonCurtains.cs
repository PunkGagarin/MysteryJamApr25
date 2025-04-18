using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Jam.Scripts.DayTime
{
    public class WagonCurtains : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int Open = Animator.StringToHash("Open");
        private static readonly int Close = Animator.StringToHash("Close");

        public event Action OnCurtainsOpened;
        public event Action OnCurtainsClosed;
        
        public void OpenCurtains() =>
            _animator.SetTrigger(Open);
        
        public void CloseCurtains() =>
            _animator.SetTrigger(Close);
        
        [UsedImplicitly]
        public void OnCurtainsOpenedEvent() 
            => OnCurtainsOpened?.Invoke();
        [UsedImplicitly]
        public void OnCurtainsClosedEvent() 
            => OnCurtainsClosed?.Invoke();
    }
}