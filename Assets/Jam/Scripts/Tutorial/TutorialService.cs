using Jam.Scripts.Camera;
using Jam.Scripts.Npc;
using Jam.Scripts.Ritual;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Tutorial
{
    public class TutorialService : MonoBehaviour
    {
        [Inject] private CameraMover _cameraMover;
        [Inject] private Character _characterController;
        [Inject] private RitualController _ritualController;
        public int TutorialStep { get; set; } = 0;

        private bool _seenRight, _seenLeft;
        
        public void TutorialEvent(int intValue)
        {
            switch (intValue)
            {
                case 0:
                    _cameraMover.UnlockCamera();
                    break;
                default:
                    Debug.LogError($"не верный туториальный ивент {intValue}");
                    break;
            }
        }
        
        private void SeenCenter()
        {
            if (_seenLeft && _seenRight)
            {
                _characterController.Talk();
                _cameraMover.OnMoveLeft   -= SeenLeft;
                _cameraMover.OnMoveRight  -= SeenRight;
                _cameraMover.OnMoveCenter -= SeenCenter;
            }
        }

        private void SeenRight()
        {
            _seenRight = true;
            _cameraMover.OnMoveRight -= SeenRight;
        }

        private void SeenLeft()
        {
            _seenLeft = true;
            _cameraMover.OnMoveLeft -= SeenLeft;
        }

        private void CheckReagents()
        {
            if (_ritualController.IsAllReagentsOnTable)
            {
                _ritualController.OnAddReagent -= CheckReagents;
                _cameraMover.MoveCameraCenter();
                _characterController.Talk();
            }
        }
        
        private void Awake()
        {
            _cameraMover.OnMoveLeft   += SeenLeft;
            _cameraMover.OnMoveRight  += SeenRight;
            _cameraMover.OnMoveCenter += SeenCenter;
            _ritualController.OnAddReagent += CheckReagents;
        }

        private void OnDestroy()
        {
            _cameraMover.OnMoveLeft   -= SeenLeft;
            _cameraMover.OnMoveRight  -= SeenRight;
            _cameraMover.OnMoveCenter -= SeenCenter;
            _ritualController.OnAddReagent -= CheckReagents;
        }
    }
}