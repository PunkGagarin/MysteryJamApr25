using Jam.Scripts.Audio.Domain;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Ritual.Components
{
    public class Component : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ComponentDefinition _componentDefinition;
        [Inject] private RitualController _ritualController;
        [Inject] private ComponentsAnimationController _componentsAnimationController;
        [Inject] private AudioService _audioService;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;

            _audioService.PlaySound(Sounds.buttonClick.ToString());
            if (_ritualController.TryAddComponent(_componentDefinition, out ComponentRoom room))
                _componentsAnimationController.PlayAnimation(_componentDefinition, transform.position, room);
        }
    }
}
