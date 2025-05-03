using System;
using DG.Tweening;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Ritual.Desk
{
    public class Disk : MonoBehaviour, IDisk, IPointerDownHandler
    {
        [SerializeField] private SpriteRenderer _visual;
        [SerializeField] private ReagentType _diskType;
        [SerializeField] private SpriteRenderer _reagentVisual;
        [SerializeField] private Sprite _sexVisual, _ageVisual, _raceVisual;

        [Inject] private ReagentAnimationController _animationController;

        private ReagentDefinition _reagentDefinition;
        private ReagentRoom _startingReagentRoom;
        
        public event Action OnDiskChanged;
        public event Action<Disk> DiskClicked;
        public bool IsLocked { get; set; }
        public ReagentDefinition ReagentInside => _reagentDefinition;

        public SpriteRenderer ReagentVisual => _reagentVisual;

        public bool TryInsertReagent(ReagentDefinition reagentDefinition, ReagentRoom startingReagentRoom)
        {
            if (_reagentDefinition != null || _diskType != reagentDefinition.ReagentType)
                return false;
            
            _reagentDefinition = reagentDefinition;
            _startingReagentRoom = startingReagentRoom;
            _reagentVisual.enabled = true;
            _reagentVisual.sprite = reagentDefinition.Visual;
            _reagentVisual.size = Vector2.one;
            OnDiskChanged?.Invoke();
            
            return true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            DiskClicked?.Invoke(this);
            if (_reagentDefinition == null || IsLocked)
                return;

            _animationController.PlayAnimationFromPosition(transform.position, _startingReagentRoom.Position, _reagentVisual.sprite, _startingReagentRoom.AppearReagent);
            ClearReagent();
        }

        public void ClearReagent()
        {
            _reagentVisual.enabled = false;
            _reagentDefinition = null;
            _startingReagentRoom = null;
            OnDiskChanged?.Invoke();
        }

        public void SetType(int number)
        {
            _visual.DOColor(Color.clear, .5f).OnComplete(() =>
            {
                _diskType = (ReagentType)number;
                _visual.sprite = _diskType switch
                {
                    ReagentType.Age => _ageVisual,
                    ReagentType.Sex => _sexVisual,
                    ReagentType.Race => _raceVisual,
                    _ => throw new ArgumentOutOfRangeException()
                };
                _visual.DOColor(Color.white, .5f);
            });
        }
    }
}