using System;
using DG.Tweening;
using UnityEngine;

namespace Jam.Scripts.Npc
{
    public class NpcMovement : MonoBehaviour
    {
        [SerializeField] private float _arriveTime = 1f;

        public void ShowNpc(Action onShow)
        {
            transform.DOScale(1, _arriveTime).OnComplete(() => onShow?.Invoke());
        }

        public void HideNpc(Action onHide)
        {
            transform.DOScale(0, _arriveTime).OnComplete(() => onHide?.Invoke());
        }
    }
}
