using Jam.Scripts.Camera;
using Jam.Scripts.Npc;
using Jam.Scripts.Ritual;
using Jam.Scripts.VFX;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Tutorial
{
    public class TutorialService : MonoBehaviour
    {
        [Inject] private CameraMover _cameraMover;
        [Inject] private Character _characterController;
        [Inject] private RitualController _ritualController;
        [Inject] private PointerFirefly _pointerFirefly;
        public int TutorialStep { get; set; } = 0;

        private bool _seenRight, _seenLeft;
        
        public void TutorialEvent(int intValue)
        {
            switch (intValue)
            {
                case 0:
                    _cameraMover.UnlockCamera();
                    _pointerFirefly.ChangeTargetTo(TargetType.RightSideScreen);
                    break;
                case 1:
                    _ritualController.StartMemoryGame();
                    _pointerFirefly.HideTillNextTarget();
                    break;
                case 3:
                    _pointerFirefly.ChangeTargetTo(TargetType.Table);   
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
            _pointerFirefly.ChangeTargetTo(TargetType.LeftSideScreen);
        }

        private void SeenLeft()
        {
            _seenLeft = true;
            _cameraMover.OnMoveLeft -= SeenLeft;
            _pointerFirefly.ChangeTargetTo(TargetType.DialogueBubble2);
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
            _ritualController.TutorialRitual += TutorialRitual;
        }

        private void TutorialRitual()
        {
            _ritualController.TutorialRitual -= TutorialRitual;
            _pointerFirefly.ChangeTargetTo(TargetType.DialogueBubble4);
            _characterController.Talk();
        }

        private void OnDestroy()
        {
            _cameraMover.OnMoveLeft   -= SeenLeft;
            _cameraMover.OnMoveRight  -= SeenRight;
            _cameraMover.OnMoveCenter -= SeenCenter;
            _ritualController.OnAddReagent -= CheckReagents;
            _ritualController.TutorialRitual -= TutorialRitual;
        }
    }
}