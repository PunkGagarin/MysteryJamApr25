using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.VFX
{
    public class PointerFirefly : MonoBehaviour
    {
        [SerializeField] private List<Transform> _targets;
        [SerializeField] private float _radius = 1.0f;
        [SerializeField] private float _speed = 1.0f;
        [SerializeField] private float _noiseAmount = 0.2f;

        private float _angle;
        private int _currentTarget = 0;
        private bool _isMovingToTarget = false;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
            {
                ChangeTarget();
            }

            if (!_isMovingToTarget)
                MoveAroundTarget();
            else
            {
                MoveToNextTarget();
            }
        }

        private void MoveAroundTarget()
        {
            _angle += _speed * Time.deltaTime;
            float x = Mathf.Cos(_angle) * _radius;
            float y = Mathf.Sin(_angle) * _radius;

            Vector2 noise = new Vector2(
                Mathf.PerlinNoise(Time.time * 2, 0) - 0.5f,
                Mathf.PerlinNoise(0, Time.time * 2) - 0.5f
            ) * _noiseAmount;

            transform.position = _targets[_currentTarget].position + new Vector3(x, y, 0) + (Vector3)noise;
        }

        private void ChangeTarget()
        {
            _isMovingToTarget = true;
            _currentTarget += 1;

            if (_currentTarget >= _targets.Count)
            {
                _currentTarget = 0;
            }
        }

        private void MoveToNextTarget()
        {
            Transform newTarget = _targets[_currentTarget];

            Vector2 noise = new Vector2(
                Mathf.PerlinNoise(Time.time * 2, 0) - 0.5f,
                Mathf.PerlinNoise(0, Time.time * 2) - 0.5f
            ) * _noiseAmount;

            float x = newTarget.position.x + noise.x;
            float y = newTarget.position.y + noise.y;

            transform.position = Vector2.Lerp(transform.position, new Vector2(x, y), _speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, newTarget.position) < 0.1f)
            {
                _isMovingToTarget = false;
            }
        }
    }
}