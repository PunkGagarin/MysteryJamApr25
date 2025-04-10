using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Camera
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private Vector3 _leftTransform, _centerTransform, _rightTransform;
        [SerializeField] private float _timeToMove;
        [SerializeField] private Button _leftButton, _rightButton;

        private CameraPosition _position;
        
        private void Start()
        {
            _position = CameraPosition.Center;
            
            _leftButton.onClick.AddListener(MoveLeft);
            _rightButton.onClick.AddListener(MoveRight);
        }

        private void MoveLeft()
        {
            _position--;

            UpdateButtons();
            MoveTarget();
        }
        
        private void MoveTarget()
        {
            switch (_position)
            {
                case CameraPosition.Left:
                    _transform.DOMove(_leftTransform, _timeToMove);
                    break;
                case CameraPosition.Center:
                    _transform.DOMove(_centerTransform, _timeToMove);
                    break;
                case CameraPosition.Right:
                    _transform.DOMove(_rightTransform, _timeToMove);
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

        private void MoveRight()
        {
            _position++;

            UpdateButtons();
            MoveTarget();
        }
    }
    
    public enum CameraPosition
    {
        Left,
        Center,
        Right
    }
}
