using System;
using DG.Tweening;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Input;
using Jam.Scripts.Manual;
using Jam.Scripts.Utils.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Camera
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Vector3 _leftTransform, _centerTransform, _rightTransform;
        [SerializeField] private float _timeToMove;
        [SerializeField] private Button _leftButton, _rightButton;

        [Inject] private AudioService _audioService;
        [Inject] private InputService _inputService;
        [Inject] private PopupManager _popupManager;
        
        private CameraPosition _position;
        public event Action OnMoveLeft, OnMoveRight, OnMoveCenter;

        public void UnlockCamera()
        {
            _leftButton.gameObject.SetActive(true);
            _rightButton.gameObject.SetActive(true);
        }

        public void MoveCameraCenter()
        {
            _position = CameraPosition.Center;
            MoveTarget();
        }
        
        private void Start()
        {
            _position = CameraPosition.Center;
            
            _leftButton.onClick.AddListener(MoveLeft);
            _rightButton.onClick.AddListener(MoveRight);
            _inputService.OnLeft += MoveLeft;
            _inputService.OnRight += MoveRight;
            _leftButton.gameObject.SetActive(false);
            _rightButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _leftButton.onClick.RemoveListener(MoveLeft);
            _rightButton.onClick.RemoveListener(MoveRight);
            _inputService.OnLeft -= MoveLeft;
            _inputService.OnRight -= MoveRight;
        }

        private void MoveLeft()
        {
            if (_popupManager.IsPopupOpen(out ManualPopup popup))
            {
                popup.Close();
            }
            
            _position--;

            if (_position < CameraPosition.Left)
            {
                _position = CameraPosition.Left;
                return;
            }
            
            MoveTarget();
        }
        private void MoveRight()
        {
            _position++;

            if (_position > CameraPosition.Right)
            {
                _position = CameraPosition.Right;
                return;
            }
            
            MoveTarget();
        }
        
        private void MoveTarget()
        {
            UpdateButtons();
            _audioService.PlaySound(Sounds.cameraMove.ToString());
            switch (_position)
            {
                case CameraPosition.Left:
                    _transform.DOMove(_leftTransform, _timeToMove).SetEase(Ease.Linear);
                    OnMoveLeft?.Invoke();
                    break;
                case CameraPosition.Center:
                    _transform.DOMove(_centerTransform, _timeToMove).SetEase(Ease.Linear);
                    OnMoveCenter?.Invoke();
                    break;
                case CameraPosition.Right:
                    _transform.DOMove(_rightTransform, _timeToMove).SetEase(Ease.Linear);
                    OnMoveRight?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateButtons()
        {
            _leftButton.interactable = _position != CameraPosition.Left;
            _rightButton.interactable = _position != CameraPosition.Right;
            
            _leftButton.gameObject.SetActive(_position != CameraPosition.Left);
            _rightButton.gameObject.SetActive(_position != CameraPosition.Right);
        }
    }
    
    public enum CameraPosition
    {
        Left,
        Center,
        Right
    }
}
