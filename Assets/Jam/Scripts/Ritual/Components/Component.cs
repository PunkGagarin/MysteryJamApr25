using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Ritual.Components
{
    public class Component : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ComponentDefinition _componentDefinition;
        [Inject] private RitualController _ritualController;
        [Inject] private ComponentsAnimationController _componentsAnimationController;

        private void Awake()
        {
            _spriteRenderer.sprite = _componentDefinition.Visual;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;

            if (_ritualController.TryAddComponent(_componentDefinition, out ComponentRoom room))
                _componentsAnimationController.PlayAnimation(_componentDefinition, transform.position, room);
        }
    }
}
