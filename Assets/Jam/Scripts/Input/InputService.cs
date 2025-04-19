using System;
using UnityEngine.InputSystem;
using Zenject;

namespace Jam.Scripts.Input
{
    public class InputService : IInitializable, IDisposable
    {
        public WagonInput WagonInput { get; private set; }

        public event Action OnLeft, OnRight;
        
        public void Initialize()
        {
            WagonInput = new WagonInput();
            WagonInput.Enable();
            WagonInput.Wagon.Enable();  

            WagonInput.Wagon.Left.started += Left;
            WagonInput.Wagon.Right.started += Right;
        }

        public void Dispose()
        {
            WagonInput.Disable();

            WagonInput.Wagon.Left.started -= Left;
            WagonInput.Wagon.Right.started -= Right;
        }
        
        private void Left(InputAction.CallbackContext context) => 
            OnLeft?.Invoke();
        
        private void Right(InputAction.CallbackContext context) =>
            OnRight?.Invoke();
    }
}