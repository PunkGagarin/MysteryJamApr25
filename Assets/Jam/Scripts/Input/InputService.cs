using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Jam.Scripts.Input
{
    public class InputService : IInitializable, IDisposable
    {
        public WagonInput WagonInput { get; private set; }

        public event Action OnLeft, OnRight;
        public event Action OnNextPage, OnPreviousPage;
        public event Action<Vector2> OnMouseDrag;
        public event Action OnMouseEndDrag;
        
        public Vector2 MousePosition { get; private set; }
        
        public void Initialize()
        {
            WagonInput = new WagonInput();
            WagonInput.Enable();
            
            WagonInput.Wagon.Enable();  
            
            WagonInput.Wagon.Left.started += Left;
            WagonInput.Wagon.Right.started += Right;
            WagonInput.Wagon.Drag.performed += Drag;
            WagonInput.Wagon.EndDrag.canceled += EndDrag;
            
            WagonInput.Manual.NextPage.started += NextPage;
            WagonInput.Manual.PrevPage.started += PreviousPage;
        }

        public void EnableWagonInput()
        {
            WagonInput.Manual.Disable();
            WagonInput.Wagon.Enable();
        }
        
        public void EnableManualInput()
        {
            WagonInput.Wagon.Disable();
            WagonInput.Manual.Enable();
        }
        
        public void Dispose()
        {
            WagonInput.Disable();

            WagonInput.Wagon.Left.started -= Left;
            WagonInput.Wagon.Right.started -= Right;
            WagonInput.Wagon.Drag.performed -= Drag;
            
            WagonInput.Manual.NextPage.started -= NextPage;
            WagonInput.Manual.PrevPage.started -= PreviousPage;
        }
        
        private void Drag(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
            OnMouseDrag?.Invoke(MousePosition);
        }

        private void Left(InputAction.CallbackContext context) => 
            OnLeft?.Invoke();
        
        private void Right(InputAction.CallbackContext context) =>
            OnRight?.Invoke();

        private void PreviousPage(InputAction.CallbackContext obj) => 
            OnPreviousPage?.Invoke();

        private void NextPage(InputAction.CallbackContext context) => 
            OnNextPage?.Invoke();

        private void EndDrag(InputAction.CallbackContext obj) => 
            OnMouseEndDrag?.Invoke();
    }
}