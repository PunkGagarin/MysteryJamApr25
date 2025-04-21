using System;
using DG.Tweening;
using UnityEngine;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    public class ReagentAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _componentSprite;
        private InventoryConfig _inventoryConfig;
        
        public bool IsPlaying { get; private set; }

        public void Initialize(InventoryConfig inventoryConfig) =>
            _inventoryConfig = inventoryConfig;

        public void StartAnimation(Vector3 startPosition, Vector3 endPosition, Sprite componentToShow, Action onAnimationEnds)
        {
            IsPlaying = true;
            _componentSprite.sprite = componentToShow;
            _componentSprite.gameObject.SetActive(true);
            transform.position = startPosition;
            transform.DOMove(endPosition, _inventoryConfig.ReagentAnimationTime)
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
