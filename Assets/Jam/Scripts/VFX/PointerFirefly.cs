using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Jam.Scripts.VFX
{
    public class PointerFirefly : MonoBehaviour
    {
        [SerializeField, Tooltip("Порядок должен совпадать с (int)TargetType")]
        private List<TargetData> _targets;
        [SerializeField] private float _movingAroundSpeed;
        [SerializeField] private float _movingToTargetSpeed;
        [SerializeField] private float _noiseAmount = 0.2f;
        [SerializeField] private float circleUpdateInterval = 0.02f;

        public int CurrentTarget { get; private set; } = (int)TargetType.Rope1;

        private float _angle;
        private bool _isMovingToTarget = false;
        private Tween _circleTween;
        private Tween _moveTween;

        private void Start()
        {
            StartMovingAroundTarget();
        }

        private void StartMovingAroundTarget()
        {
            _isMovingToTarget = false;
            _circleTween?.Kill();

            _circleTween = DOVirtual.DelayedCall(circleUpdateInterval, UpdateCirclePosition)
                .SetLoops(-1);
        }

        private void UpdateCirclePosition()
        {
            if (_isMovingToTarget || CurrentTarget >= _targets.Count)
                return;

            _angle += _movingAroundSpeed * circleUpdateInterval;
            float radius = GetCurrentRadius();
            float x = Mathf.Cos(_angle) * radius;
            float y = Mathf.Sin(_angle) * radius;

            Vector2 noise = new Vector2(
                Mathf.PerlinNoise(Time.time * 2f, 0f) - 0.5f,
                Mathf.PerlinNoise(0f, Time.time * 2f) - 0.5f
            ) * _noiseAmount;

            Vector3 newPos = _targets[CurrentTarget].Target.position + new Vector3(x, y, 0) + (Vector3)noise;

            transform.DOMove(newPos, circleUpdateInterval).SetEase(Ease.Linear);
        }

        public void ChangeTargetTo(TargetType target)
        {
            
            if (CurrentTarget >= (int)target)
                return;
            
            if (target == TargetType.Finish || CurrentTarget == (int)TargetType.Finish)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            _isMovingToTarget = true;
            _circleTween?.Kill();

            var targetData = _targets.Find(x => x.Type == target);
            CurrentTarget = (int)targetData.Type;
            if (CurrentTarget >= _targets.Count)
                CurrentTarget = 0;

            MoveToNewTargetSmoothly();
        }

        private void MoveToNewTargetSmoothly()
        {
            Transform targetTransform = _targets[CurrentTarget].Target;
            Vector3 noise = new Vector3(
                Mathf.PerlinNoise(Time.time * 2f, 0f) - 0.5f,
                Mathf.PerlinNoise(0f, Time.time * 2f) - 0.5f,
                0f
            ) * _noiseAmount;

            Vector3 targetPos = targetTransform.position + noise;

            float distance = Vector3.Distance(transform.position, targetPos);
            float duration = distance / _movingToTargetSpeed;

            _moveTween?.Kill();
            _moveTween = transform.DOMove(targetPos, duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(StartMovingAroundTarget);
        }

        private float GetCurrentRadius()
        {
            if (CurrentTarget >= 0 && CurrentTarget < _targets.Count)
                return _targets[CurrentTarget].Radius;

            return 1.0f;
        }

        public void HideTillNextTarget() =>
            gameObject.SetActive(false);
    }
}

public enum TargetType
{
    Rope1 = 1,
    Character = 2,
    DialogueBubble1 = 3,
    RightSideScreen = 4,
    LeftSideScreen = 5,
    DialogueBubble2 = 6,
    Manual = 7,
    FirstReagent = 8,
    SecondReagent = 9,
    ThirdReagent = 10,
    DialogueBubble3 = 11,
    Table = 12,
    DialogueBubble4 = 13,
    Rope2 = 14,
    Manual2 = 15,
    Finish = 16,
}

[Serializable]
public class TargetData
{
    [SerializeField] private TargetType _type;
    [SerializeField] private Transform _target;
    [SerializeField] private float _radius = 1.0f;
    public TargetType Type => _type;
    public Transform Target => _target;
    public float Radius => _radius;
}