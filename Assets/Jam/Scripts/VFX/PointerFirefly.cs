using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.VFX
{
    public class PointerFirefly : MonoBehaviour
    {
        [SerializeField] private List<TargetData> _targets;
        [SerializeField] private float _radius = 1.0f;
        [SerializeField] private float _movingArounSpeed = 1.0f;
        [SerializeField] private float _movingToTargetSpeed = 1.0f;
        [SerializeField] private float _noiseAmount = 0.2f;

        private float _angle;
        public int CurrentTarget { get; private set; } = (int)TargetType.Rope;
        private bool _isMovingToTarget = false;

        private void Update()
        {
            if (!_isMovingToTarget)
                MoveAroundTarget();
            else
            {
                MoveToNextTarget();
            }
        }

        private void MoveAroundTarget()
        {
            _angle += _movingArounSpeed * Time.deltaTime;
            float x = Mathf.Cos(_angle) * _radius;
            float y = Mathf.Sin(_angle) * _radius;

            Vector2 noise = new Vector2(
                Mathf.PerlinNoise(Time.time * 2, 0) - 0.5f,
                Mathf.PerlinNoise(0, Time.time * 2) - 0.5f
            ) * _noiseAmount;

            transform.position = _targets[CurrentTarget].Target.position + new Vector3(x, y, 0) + (Vector3)noise;
        }

        public void ChangeTargetTo(TargetType target)
        {
            if (target == TargetType.None || CurrentTarget == (int)TargetType.None)
            {
                gameObject.SetActive(false);
                return;
            }

            _isMovingToTarget = true;
            var targetData = _targets.Find(x => x.Type == target);
            CurrentTarget = (int)targetData.Type;

            if (CurrentTarget >= _targets.Count)
            {
                CurrentTarget = 0;
            }
        }

        private void MoveToNextTarget()
        {
            Transform newTarget = _targets[CurrentTarget].Target;

            Vector2 noise = new Vector2(
                Mathf.PerlinNoise(Time.time * 2, 0) - 0.5f,
                Mathf.PerlinNoise(0, Time.time * 2) - 0.5f
            ) * _noiseAmount;

            float x = newTarget.position.x + noise.x;
            float y = newTarget.position.y + noise.y;

            transform.position =
                Vector2.Lerp(transform.position, new Vector2(x, y), _movingToTargetSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, newTarget.position) < 0.1f)
            {
                _isMovingToTarget = false;
            }
        }
    }
}

public enum TargetType
{
    None = 0,
    Rope = 1,
    Character = 2,
    Book = 3,
    Reagents = 4,
    Table = 5
}

[Serializable]
public class TargetData
{
    [SerializeField] private TargetType type;
    [SerializeField] private Transform target;

    public TargetType Type => type;
    public Transform Target => target;
}