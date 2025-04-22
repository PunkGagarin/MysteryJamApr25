using DG.Tweening;
using UnityEngine;

namespace Jam.Scripts.Npc
{
    public class PointerShower : MonoBehaviour
    {
        [SerializeField] private Transform _pointer;
        [SerializeField] private float _topPositionY;
        [SerializeField] private float _bottomPositionY;

        public void Show()
        {
            _pointer.gameObject.SetActive(true);
            _pointer.DOLocalMoveY(_topPositionY, 0f);
            _pointer.DOLocalMoveY(_bottomPositionY, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public void Hide()
        {
            _pointer.DOKill();
            _pointer.gameObject.SetActive(false);
        }
        
    }
}