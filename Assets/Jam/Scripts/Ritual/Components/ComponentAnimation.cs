using System;
using DG.Tweening;
using UnityEngine;

namespace Jam.Scripts.Ritual.Components
{
    public class ComponentAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _componentSprite;
        [SerializeField] private float _timeToArrive;
        
        public bool IsPlaying { get; private set; }

        public void StartAnimation(Vector3 startPosition, Vector3 endPosition, Sprite componentToShow, Action onAnimationEnds)
        {
            IsPlaying = true;
            _componentSprite.sprite = componentToShow;
            _componentSprite.gameObject.SetActive(true);
            transform.position = startPosition;
            transform.DOMove(endPosition, _timeToArrive)
                .SetEase(Ease.OutExpo)
                .OnComplete(() =>
                {
                    onAnimationEnds?.Invoke();
                    _componentSprite.gameObject.SetActive(false);
                    IsPlaying = false;
                });
        }
    }
}
