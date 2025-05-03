using Jam.Scripts.Input;
using Jam.Scripts.Ritual.Desk;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    public class ReagentDragger : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _reagentVisual;
        [SerializeField] private LayerMask _diskMask;

        [Inject] private InputService _inputService;
        [Inject] private ReagentAnimationController _reagentAnimation;
        
        private ReagentDefinition _draggingReagent;
        private ReagentRoom _startingReagentRoom;
        private bool _isDragging;
        
        public void StartDrag(ReagentRoom reagentRoom, ReagentDefinition reagentInside)
        {
            _isDragging = true;
            UpdatePosition(_inputService.MousePosition);
            _reagentVisual.sprite = reagentInside.Visual;
            _reagentVisual.size = Vector2.one;
            _draggingReagent = reagentInside;
            _startingReagentRoom = reagentRoom;
            _reagentVisual.enabled = true;
        }

        private void OnEndDrag()
        {
            if (!_isDragging)
                return;

            _isDragging = false;
            _reagentVisual.enabled = false;
            
            Vector2 mousePosition = UnityEngine.Camera.main.ScreenToWorldPoint(_inputService.MousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0f, _diskMask);

            if (hit.collider != null)
            {
                var disk = hit.collider.GetComponent<IDisk>();
                if (disk != null)
                {
                    if (!disk.TryInsertReagent(_draggingReagent, _startingReagentRoom))
                    {
                        ReturnReagent();
                    }
                }
                else
                {
                    ReturnReagent();
                }
            }
            else
            {
                ReturnReagent();
            }
        }

        private void ReturnReagent()
        {
            _reagentAnimation.PlayAnimationFromPosition(
                transform.position,
                _startingReagentRoom.Position, 
                _draggingReagent.Visual, 
                _startingReagentRoom.AppearReagent);
        }

        private void Awake()
        {
            _inputService.OnMouseDrag += OnDrag;
            _inputService.OnMouseEndDrag += OnEndDrag;
        }

        private void OnDrag(Vector2 position)
        {
            if (!_isDragging)
                return;

            UpdatePosition(position);
        }

        private void UpdatePosition(Vector2 position)
        {
            Vector3 worldCoord = UnityEngine.Camera.main.ScreenToWorldPoint(position);
            worldCoord.z = 0;
            transform.position = worldCoord;
        }

        private void OnDestroy()
        {
            _inputService.OnMouseDrag -= OnDrag;
            _inputService.OnMouseEndDrag -= OnEndDrag;
        }
    }
}