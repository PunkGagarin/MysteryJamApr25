using DG.Tweening;
using UnityEngine;

namespace Jam.Scripts.Npc
{
    public class PointerShower : MonoBehaviour
    {
        [SerializeField] private Transform _pointer;

        public void Show()
        {
            _pointer.gameObject.SetActive(true);
            _pointer.DOLocalMoveY(3.25f, 0f);
            _pointer.DOLocalMoveY(2.75f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public void Hide()
        {
            _pointer.DOKill();
            _pointer.gameObject.SetActive(false);
        }
        
    }
}